using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Herlitz.BankID
{
    public class SignRequest : AuthRequest, ISignRequest
    {

        public SignRequest(IRequirement requirement, ISSNHelper ssnHelper) : base(requirement, ssnHelper)
        {
        }

        #region Implementation of ISignRequest

        private string _userVisibleData;
        /// <summary>
        /// The text to be displayed and signed. String. The text can be formatted using CR, LF and CRLF for new lines.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "userVisibleData")]
        public string UserVisibleData
        {
            // The text must be encoded as UTF-8 and then base 64 encoded. 1--40 000 characters after base 64 encoding
            get => Convert.ToBase64String(Encoding.UTF8.GetBytes(_userVisibleData));
            set => _userVisibleData = value;
        }

        private string _userNonVisibleData;
        /// <summary>
        /// Data not displayed to the user.
        /// </summary>
        [JsonProperty(PropertyName = "userNonVisibleData", NullValueHandling = NullValueHandling.Ignore)]
        public string UserNonVisibleData
        {
            // The value must be base 64-encoded. 1-200 000 characters after base 64-encoding
            get => string.IsNullOrEmpty(_userNonVisibleData) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(_userNonVisibleData));
            set => _userNonVisibleData = value;
        }

        #endregion
    }
}