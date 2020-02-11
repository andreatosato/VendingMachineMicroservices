using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Service.Authentications.API.ViewModels
{
    public class LoginRequest
    {
        [Required]
        public string Grant_Type { get; set; }

        public string Scopes { get; set; }

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
