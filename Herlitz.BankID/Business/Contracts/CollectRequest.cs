using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Herlitz.BankID
{
    public class CollectRequest : ICollectRequest
    {
        #region Implementation of ICollectRequest

        /// <summary>
        /// The orderRef returned from auth or sign
        /// </summary>
        [JsonProperty(PropertyName = "orderRef")]
        public string OrderRef { get; set; }


        #endregion
    }
}