using System;
using Herlitz.BankID.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace Herlitz.BankID
{
    public class BankIDServiceSettings
    {
        private readonly IWebHostEnvironment _environment;
        private readonly BankIDConfig _bankIdConfig;

        public Uri ApiUrl { get; private set; }

        /// <summary>
        /// RpCert file, Only for development
        /// </summary>
        public string RpCert { get; private set; }

        /// <summary>
        /// Server certifikate file, Only for development
        /// </summary>
        public string CaCert { get; private set; }
        public string RpCertThumbprint { get; private set; }
        public string CaCertThumbprint { get; private set; }

        public BankIDServiceSettings(IWebHostEnvironment environment, IOptions<BankIDConfig> bankIdConfig)
        {
            _environment = environment;
            _bankIdConfig = bankIdConfig.Value;

            ApiUrl = new Uri(_bankIdConfig.ApiUrl);
            RpCertThumbprint = _bankIdConfig.RPCertificateThumbprint;
            CaCertThumbprint = _bankIdConfig.ServerCertificateThumbprint;

            if (_environment.IsDevelopment())
            {
                RpCert = "FPTestcert2_20150818_102329.pfx";
                CaCert = "bankid.cer";
            }
        }
    }

}
