using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Shared.Authentication
{
    public class JwtSettings
    {
        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpirationMinutes { get; set; }
    }
}
