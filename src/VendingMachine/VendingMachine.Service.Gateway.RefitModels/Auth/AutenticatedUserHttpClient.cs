using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Service.Gateway.RefitModels.Auth
{
    public class AutenticatedUserHttpClient : IAuthUserClient
    {
        private readonly IAuthenticationApi authenticationApi;

        public AutenticatedUserHttpClient(IAuthenticationApi authenticationApi)
        {
            this.authenticationApi = authenticationApi;
        }

        private async Task<string> GetToken(string username, string password)
        {
            // The AcquireTokenAsync call will prompt with a UI if necessary
            // Or otherwise silently use a refresh token to return
            // a valid access token	
            var response = await authenticationApi.LoginClientAsync(new LoginRequest() 
            { 
                Grant_Type = "password",
                Scopes = new [] {"Machine.Api", "Product.Api", "Order.Api"}.ToList(),
                Username = username,
                Password = password
            });
            return response.token;
        }

        public HttpClient GetClient(string baseUrl, string username, string password)
        {
            
            var authHandler = new AuthenticatedHttpClientHandler(() => GetToken(username, password))
            {
                Version = new Version("2.0")
            };

            var client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(baseUrl),
                //DefaultRequestVersion = new Version(2, 0), // Not in use
            };
            return client;
        }

        private class AuthResponse
        {
            public string token { get; set; }
        }
    }
}
