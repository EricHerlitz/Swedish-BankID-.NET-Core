using Herlitz.BankID.Core;
using Microsoft.Extensions.Options;
using QRCoder;

namespace Herlitz.BankID.Business.Helpers
{
    public interface IQRCodeFactory
    {
        string GenerateBankIDQRCode(string autoStartToken);
    }

    public class QRCodeFactory : IQRCodeFactory
    {
        private readonly BankIDConfig _bankIdConfig;

        public QRCodeFactory(IOptions<BankIDConfig> bankIdConfig)
        {
            _bankIdConfig = bankIdConfig.Value;
        }

        public string GenerateBankIDQRCode(string autoStartToken)
        {
            var bidUrl = $"bankid:///?autostarttoken={autoStartToken.Trim()}";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(bidUrl, QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new Base64QRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(_bankIdConfig.QRPixels);
                }
            }
        }
    }
}
