namespace Herlitz.BankID
{
    public interface ISignRequest : IAuthRequest
    {
        string UserVisibleData { get; set; }

        string UserNonVisibleData { get; set; }
    }
}