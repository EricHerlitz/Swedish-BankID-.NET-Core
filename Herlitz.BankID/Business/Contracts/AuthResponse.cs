using Newtonsoft.Json;

namespace Herlitz.BankID
{
    public class AuthResponse : IAuthResponse
    {

        #region Implementation of IAuthResponse

        /// <summary>
        /// Used as reference to this order when the client is started automatically
        /// </summary>
        public string AutoStartToken { get; set; }

        /// <summary>
        /// Used to collect the status of the order
        /// </summary>
        public string OrderRef { get; set; }

        #endregion
    }
}