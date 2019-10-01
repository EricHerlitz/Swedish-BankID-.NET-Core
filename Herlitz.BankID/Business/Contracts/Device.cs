namespace Herlitz.BankID
{
    public class Device : IDevice
    {
        #region Implementation of IDevice

        /// <summary>
        /// The IP address of the user agent as the BankID server discovers it
        /// </summary>
        public string IpAddress { get; set; }

        #endregion
    }
}