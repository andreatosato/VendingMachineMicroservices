using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public interface IAuthenticationApi
    {
        [Post("connect/login")]
        Task<string> LoginAsync();
    }
}
