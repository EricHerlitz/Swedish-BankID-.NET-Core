using System;
using System.Threading.Tasks;

namespace Herlitz.BankID
{
    public class BankIDService : IBankIDService
    {
        private readonly IBankIDHttpClientService _bankIdHttpClientService;
        
        public BankIDService(IBankIDHttpClientService bankIdHttpClientService)
        {
            _bankIdHttpClientService = bankIdHttpClientService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IAuthResponse> Auth(IAuthRequest request)
        {
            return await _bankIdHttpClientService.RequestClient<AuthResponse, IAuthRequest>(request, "auth");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IAuthResponse> Sign(ISignRequest request)
        {
            return await _bankIdHttpClientService.RequestClient<AuthResponse, ISignRequest>(request, "sign");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ICollectResponse> Collect(ICollectRequest request)
        {
            return await _bankIdHttpClientService.RequestClient<CollectResponse, ICollectRequest>(request, "collect");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Cancel(ICancelRequest request)
        {
            return (await _bankIdHttpClientService.RequestClient<object, ICancelRequest>(request, "cancel")).ToString().Equals("{}");
        }

    }
}