using System.Net.Http;

namespace Herlitz.BankID
{
    public interface IAuthRequest
    {
        string EndUserIp { get; set; }

        string PersonalNumber { get; set; }
    }
}