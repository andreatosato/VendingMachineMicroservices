using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Service.Authentications.API.ViewModels
{
    public class LoginClient
    {
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}
