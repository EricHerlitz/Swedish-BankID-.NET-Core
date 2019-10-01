namespace Herlitz.BankID
{
    public class CollectResponse : ICollectResponse
    {

        #region Implementation of ICollectResponse

        /// <summary>
        /// The orderRef in question
        /// </summary>
        public string OrderRef { get; set; }

        /// <summary>
        /// pending: The order is being processed. hintCode describes the status of the order.
        /// failed: Something went wrong with the order. hintCode describes the error.
        /// complete: The order is complete. completionData holds user information
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Only present for pending and failed orders
        /// </summary>
        public string HintCode { get; set; }

        /// <summary>
        /// Only present for complete orders
        /// </summary>
        public CompletionData CompletionData { get; set; }

        #endregion
    }
}