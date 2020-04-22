using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Herlitz.BankID
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
            JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
    }
}
