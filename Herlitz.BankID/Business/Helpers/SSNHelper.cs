using System;
using System.Text.RegularExpressions;

namespace Herlitz.BankID
{
    public class SSNHelper : ISSNHelper
    {
        #region Implementation of ISSNHelper

        /// <summary>
        /// Method to collect a valid SSN
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>String with yyyymmddnnnn formatted number</returns>
        public string EnsureSSN(string ssn)
        {
            string ssnRegex = @"^(\d{6}|\d{8})[-|(\s)]{0,1}\d{4}$";

            if (string.IsNullOrEmpty(ssn) || !Regex.Match(ssn, ssnRegex).Success)
            {
                throw new ArgumentException("Not a valid SSN");
            }

            // Remove any dash
            ssn = ssn.Replace("-", "");

            // if ten digits we are missing the century
            if (ssn.Length <= 10)
            {
                int year = int.Parse(ssn.Substring(0, 2));

                // set the correct century digit
                ssn = year > int.Parse(DateTime.Now.ToString("yy")) ? string.Concat("19", ssn) : string.Concat("20", ssn);
            }

            return ssn;
        }

        #endregion
    }
}
