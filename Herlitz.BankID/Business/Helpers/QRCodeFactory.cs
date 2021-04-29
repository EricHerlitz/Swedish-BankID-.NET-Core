using QRCoder;

namespace Herlitz.BankID.Business.Helpers
{
    public interface IQRCodeFactory
    {
        string GenerateBankIDQRCode(string autoStartToken);
    }

    public class QRCodeFactory : IQRCodeFactory
    {
        public string GenerateBankIDQRCode(string autoStartToken)
        {
            var bidUrl = $"bankid:///?autostarttoken={autoStartToken}";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(bidUrl, QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new Base64QRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20);
                }
            }
        }
    }
}
