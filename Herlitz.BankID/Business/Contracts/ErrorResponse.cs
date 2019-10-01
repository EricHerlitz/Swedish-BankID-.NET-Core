namespace Herlitz.BankID
{
    public class ErrorResponse : IErrorResponse
    {
        #region Implementation of IErrorResponse

        public string ErrorCode { get; set; }
        public string Details { get; set; }

        #endregion
    }
}