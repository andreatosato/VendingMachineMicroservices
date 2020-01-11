using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Service.Authentications.API.ViewModels
{
    public class AuthResponse
    {
        public string Token { get; }

        public DateTime Expiration { get; }

        public AuthResponse(string token, DateTime expiration)
        {
            Token = token;
            Expiration = expiration;
        }
    }
}
