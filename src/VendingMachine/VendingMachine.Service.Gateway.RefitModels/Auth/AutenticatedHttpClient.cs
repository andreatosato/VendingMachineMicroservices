using System;
using System.Net.Http;
using System.Threading.Tasks;
using VendingMachine.Service.Gateway.RefitModels.Auth;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public class AutenticatedHttpClient : IAuthClient
    {
        private readonly IAuthenticationApi authenticationApi;

        public AutenticatedHttpClient(IAuthenticationApi authenticationApi)
        {
            this.authenticationApi = authenticationApi;
        }

        private async Task<string> GetToken()
        {
            // The AcquireTokenAsync call will prompt with a UI if necessary
            // Or otherwise silently use a refresh token to return
            // a valid access token	
            var response = await authenticationApi.LoginClientTokenAsync(new LoginClient() { ClientName = "Worker", ClientSecret = "%&&(78045r" });
            return response.token;
        }

        public HttpClient GetClient(string baseUrl)
        {
            var client = new HttpClient(new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new Uri(baseUrl),
                DefaultRequestVersion = new Version(2, 0),
            };
            return client;
        }

        private class AuthResponse
        {
            public string token { get; set; }
        }
    }


}
