using System;
using System.Linq;
using Herlitz.BankID.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Herlitz.BankID
{
    /// <summary>
    /// RP may use the requirement parameter to describe how the signature must be created and verified.
    /// A typical use case is to require Mobile BankID or a special card reader.
    /// A requirement can be set for both auth and sign orders
    /// </summary>
    public class Requirement : IRequirement
    {
        private readonly BankIDConfig _bankIdConfig;

        public Requirement(IOptions<BankIDConfig> bankIdConfig)
        {
            _bankIdConfig = bankIdConfig.Value;

            AllowFingerprint = _bankIdConfig.AllowFingerprint;


            if (_bankIdConfig.CertificatePolicies.Any())
            {
                //CertificatePolicies = _bankIdConfig.CertificatePolicies;
            }

        }

        #region Implementation of IRequirement

        /// <summary>
        /// "class1" - (default). The transaction must be performed using a card reader where
        /// the PIN code is entered on the computers keyboard, or a card reader of higher class.
        /// "class2" - The transaction must be performed using a card reader where the PIN
        /// code is entered on the reader, or a reader of higher class.
        /// no value -  defaults to "class1". This condition should be combined with a
        /// certificatePolicies for a smart card to avoid undefined behavior
        /// </summary>
        [JsonProperty(PropertyName = "cardReader", NullValueHandling = NullValueHandling.Ignore)]
        public string CardReader { get; set; }

        /// <summary>
        /// The oid in certificate policies in the user certificate. List of String.
        /// One wildcard "*" is allowed from position 5 and forward ie. 1.2.752.78.*
        /// The values for production BankIDs are:
        ///     "1.2.752.78.1.1" - BankID on file
        ///     "1.2.752.78.1.2" - BankID on smart card
        ///     "1.2.752.78.1.5" - Mobile BankID
        ///     "1.2.752.71.1.3" - Nordea e-id on file and on smart card.
        /// The values for test BankIDs are:
        ///     "1.2.3.4.5" - BankID on file
        ///     "1.2.3.4.10" - BankID on smart card
        ///     "1.2.3.4.25" - Mobile BankID
        ///     "1.2.752.71.1.3" - Nordea e-id on file and on smart card.
        ///     "1.2.752.60.1.6" - Test BankID for some BankID Banks 
        /// </summary>
        [JsonProperty(PropertyName = "certificatePolicies", NullValueHandling = NullValueHandling.Ignore)]
        public string[] CertificatePolicies { get; set; }

        /// <summary>
        /// If issuer is not defined all relevant BankID and Nordea issuers are allowed.
        /// The cn (common name) of the issuer. List of String. Wildcards are not allowed.
        /// Nordea values for production:
        ///     "Nordea CA for Smartcard users 12" - E-id on smart card issued by Nordea CA. "
        ///     "Nordea CA for Softcert users 13" - E-id on file issued by Nordea CA
        /// Example Nordea values for test:
        ///     "Nordea Test CA for Smartcard users 12" - E-id on smart card issued by Nordea CA.
        ///     "Nordea Test CA for Softcert users 13" - E-id on file issued by Nordea CA
        /// </summary>
        [JsonProperty(PropertyName = "issuerCn", NullValueHandling = NullValueHandling.Ignore)]
        public string IssuerCn { get; set; }

        /// <summary>
        /// The client does not need to be started using autoStartToken
        /// If present, and set to true, the client must have been started using the autoStartToken.
        /// To be used if it is important that the BankID App is on the same device as the RP service.
        /// If this requirement is omitted, the client does not need to be started using autoStartToken.
        /// </summary>
        [JsonProperty(PropertyName = "autoStartTokenRequired", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoStartTokenRequired { get; set; }

        /// <summary>
        /// Users of iOS and Android devices may use fingerprint for authentication and signing if the
        /// device supports it and the user configured the device to use it. Boolean. No other devices
        /// are supported at this point. If set to true, the users are allowed to use fingerprint.
        /// If set to false, the users are not allowed to use fingerprint
        /// </summary>
        [JsonProperty(PropertyName = "allowFingerprint", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowFingerprint { get; set; }

        #endregion
    }
}