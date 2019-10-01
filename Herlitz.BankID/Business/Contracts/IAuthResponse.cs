namespace Herlitz.BankID
{
    public interface IAuthResponse
    {
        string AutoStartToken { get; set; }

        string OrderRef { get; set; }

    }
}