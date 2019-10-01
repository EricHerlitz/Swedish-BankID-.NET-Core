using System.Net.Http;

namespace Herlitz.BankID
{
    public interface ICancelRequest 
    {
        string OrderRef { get; set; }
    }
}