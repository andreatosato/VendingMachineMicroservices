using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Gateway.RefitModels.Auth;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public interface IAuthenticationApi
    {
        [Post("/connect/token")]
        Task<AuthResponse> LoginClientAsync([Body(BodySerializationMethod.UrlEncoded)]LoginRequest loginRequest);

        [Post("/connect/clienttoken")]
        Task<AuthResponse> LoginClientTokenAsync([Body] LoginClient loginRequest);
    }

    public class AuthResponse
    {
        public string token { get; set; }
    }

    //public interface IRegisterUserApi
    //{
    //    [Post("connect/register")]
    //    Task<string> LoginAsync();
    //}
}
