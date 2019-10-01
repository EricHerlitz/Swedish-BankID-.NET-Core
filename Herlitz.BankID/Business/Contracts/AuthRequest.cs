using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Herlitz.BankID
{
    public class AuthRequest : IAuthRequest
    {
        private readonly ISSNHelper _ssnHelper;
        private string _personalNumber;

        /// <summary>
        /// Requirements on how the auth or sign order must be performed
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IRequirement requirement { get; }

        public AuthRequest(IRequirement requirement, ISSNHelper ssnHelper)
        {
            _ssnHelper = ssnHelper;
            this.requirement = requirement;
        }

        #region Implementation of IAuthRequest

        /// <summary>
        /// The user IP address as seen by RP. String. IPv4 and IPv6 is allowed.
        /// Note the importance of using the correct IP address. It must be the IP address
        /// representing the user agent (the end user device) as seen by the RP.
        /// If there is a proxy for inbound traffic, special considerations may need to be
        /// taken to get the correct address. In some use cases the IP address is not available,
        /// for instance for voice based services. In this case, the internal representation
        /// of those systems IP address is ok to use. 
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "endUserIp")]
        public string EndUserIp { get; set; }

        /// <summary>
        /// The personal number of the user. String. 12 digits. Century must be included.
        /// If the personal number is excluded, the client must be started with the autoStartToken returned in the response 
        /// </summary>
        [JsonProperty(PropertyName = "personalNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PersonalNumber
        {
            get => _personalNumber;
            set => _personalNumber = _ssnHelper.EnsureSSN(value);
        }

        #endregion
    }
}