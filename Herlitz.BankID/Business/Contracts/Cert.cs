namespace Herlitz.BankID
{
    /// <summary>
    /// Note: notBefore and notAfter are the number of milliseconds since the UNIX Epoch, a.k.a.
    /// "UNIX time" in milliseconds. It was chosen over ISO8601 for its simplicity and lack of error
    /// prone conversions to/from string representations on the server and client side
    /// </summary>
    public class Cert : ICert
    {
        #region Implementation of ICert

        /// <summary>
        /// End of validity of the Users BankID. Unix ms
        /// </summary>
        public string NotAfter { get; set; }

        /// <summary>
        /// Start of validity of the users BankID. Unix ms. 
        /// </summary>
        public string NotBefore { get; set; }

        #endregion
    }
}