using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Herlitz.BankID
{
    public class BankIDException : HttpRequestException
    {
        public BankIDException(HttpStatusCode httpStatusCode, IErrorResponse errorResponse, string message)
            : this(httpStatusCode, errorResponse, message, null)
        {
        }

        public BankIDException(HttpStatusCode httpStatusCode, IErrorResponse errorResponse, string message, Exception inner)
            : base(message: message, inner: null)
        {
        }
    }
}