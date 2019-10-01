using Newtonsoft.Json;

namespace Herlitz.BankID
{
    /// <summary>
    /// The completionData includes the signature, user information and the OCSP response.
    /// </summary>
    public class CompletionData : ICompletionData
    {
        #region Implementation of ICompletionData

        /// <summary>
        /// The OCSP response. String. Base64-encoded. The OCSP response is signed by a
        /// certificate that has the same issuer as the certificate being verified.
        /// The OSCP response has an extension for Nonce. The nonce is calculated as:
        ///     SHA-1 hash over the base 64 XML signature encoded as UTF-8.
        ///     12 random bytes is added after the hash
        ///     The nonce is 32 bytes (20 + 12) 
        /// </summary>
        public string OcspResponse { get; set; }

        /// <summary>
        /// The content of the signature is described in BankID Signature Profile specification. Base64-encoded. XML signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Information related to the users certificate (BankID)
        /// </summary>
        public Cert Cert { get; set; }

        /// <summary>
        /// Information related to the device
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Information related to the user
        /// </summary>
        public User User { get; set; }

        #endregion

        /// <summary>
        /// Generated access token (if requested)
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Generated identity token (if requested)
        /// </summary>
        public string IdentityToken { get; set; }
    }
}