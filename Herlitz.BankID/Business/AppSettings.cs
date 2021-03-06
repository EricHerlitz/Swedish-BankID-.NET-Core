﻿using System;

namespace Herlitz.BankID.Core
{
    public class BankIDConfig
    {
        public string ServerCertificateThumbprint { get; set; }
        public string RPCertificateThumbprint { get; set; }
        public string ApiUrl { get; set; }
        public string[] CertificatePolicies { get; set; }
        public bool AllowFingerprint { get; set; }
        public int QRPixels { get; set; }
    }

    public class CacheOptions
    {
        public bool Allow { get; set; }

        public int Timeout { get; set; }
    }
}
