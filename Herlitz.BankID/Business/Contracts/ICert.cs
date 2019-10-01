namespace Herlitz.BankID
{
    public interface ICert
    {
        string NotAfter { get; set; }
        string NotBefore { get; set; }
    }
}