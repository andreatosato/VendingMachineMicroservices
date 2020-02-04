using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VendingMachine.Service.Gateway.RefitModels.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Grant_Type { get; set; }

        public List<string> Scopes { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class LoginClient
    {
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
    }
}
