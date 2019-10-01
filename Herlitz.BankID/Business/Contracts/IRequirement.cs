namespace Herlitz.BankID
{
    public interface IRequirement
    {
        string CardReader { get; set; }
        string[] CertificatePolicies { get; set; }
        string IssuerCn { get; set; }
        bool? AutoStartTokenRequired { get; set; }
        bool? AllowFingerprint { get; set; }
    }
}