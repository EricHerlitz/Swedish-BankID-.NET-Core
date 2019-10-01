namespace Herlitz.BankID
{
    public interface ICompletionData
    {
        string OcspResponse { get; set; }
        string Signature { get; set; }
        Cert Cert { get; set; }
        Device Device { get; set; }
        User User { get; set; }
    }
}