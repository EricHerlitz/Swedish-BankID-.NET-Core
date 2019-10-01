using System.Threading.Tasks;

namespace Herlitz.BankID
{
    public interface IBankIDHttpClientService
    {
        Task<TResponse> RequestClient<TResponse, TRequest>(TRequest request, string url);
    }
}