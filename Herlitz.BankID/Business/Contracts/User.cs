namespace Herlitz.BankID
{
    public class User : IUser
    {
        #region Implementation of IUser

        /// <summary>
        /// The given name of the user
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        ///  The given name and surname of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The personal number (SSN)
        /// </summary>
        public string PersonalNumber { get; set; }

        /// <summary>
        /// The surname of the user
        /// </summary>
        public string Surname { get; set; }

        #endregion
    }
}