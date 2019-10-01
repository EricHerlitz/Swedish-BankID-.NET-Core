namespace Herlitz.BankID
{
    public interface IErrorResponse
    {
        string ErrorCode { get; set; }

        string Details { get; set; }
    }
}