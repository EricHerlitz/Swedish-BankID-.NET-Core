using System;
using System.Collections.Generic;
using System.Text;

namespace Herlitz.BankID
{
    public interface ISSNHelper
    {
        string EnsureSSN(string ssn);
    }
}
