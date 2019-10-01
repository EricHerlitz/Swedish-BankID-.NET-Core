using System.Net;

namespace Herlitz.BankID
{
    public interface IStatusHandler
    {
        string GetStatus(string hintCode, bool rpAutoStart = false);

        string GetError(HttpStatusCode httpStatus, IErrorResponse errorResponse = null);
    }
}