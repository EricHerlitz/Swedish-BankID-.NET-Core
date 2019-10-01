using System.Net.Http;

namespace Herlitz.BankID
{
    public interface ICollectRequest
    {
        string OrderRef { get; set; }
    }
}