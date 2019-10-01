using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Herlitz.BankID
{
    public class BankIDHttpClientService : IBankIDHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStatusHandler _statusHandler;
        private readonly BankIDServiceSettings _bankIdServiceSettings;
        private const string ClientName = "BankIDClient";

        public BankIDHttpClientService(
            IHttpClientFactory httpClientFactory, 
            IStatusHandler statusHandler, 
            BankIDServiceSettings bankIdServiceSettings)
        {
            _httpClientFactory = httpClientFactory;
            _statusHandler = statusHandler;
            _bankIdServiceSettings = bankIdServiceSettings;
        }

        private StringContent Content<TRequest>(TRequest request)
        {
            return new CustomStringContent(content: JsonConvert.SerializeObject(request), encoding: Encoding.UTF8);
        }


        public async Task<TResponse> RequestClient<TResponse, TRequest>(TRequest request, string url)
        {
            using (HttpClient client = _httpClientFactory.CreateClient(ClientName))
            {
                client.BaseAddress = _bankIdServiceSettings.ApiUrl;

                using (var res = await client.PostAsync(requestUri: $"{client.BaseAddress}/{url}", content: Content(request)))
                {
                    using (HttpContent content = res.Content)
                    {
                        if (res.StatusCode == HttpStatusCode.OK)
                        {
                            return await content.ReadAsAsync<TResponse>();
                        }

                        var errorResponse = await content.ReadAsAsync<ErrorResponse>();
                        throw new BankIDException(res.StatusCode, errorResponse, _statusHandler.GetError(res.StatusCode, errorResponse));
                    }
                }
            }
        }
    }
}