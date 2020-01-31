using System.Net.Http;
using System.Threading.Tasks;

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
            var token = await authenticationApi.LoginAsync();
            return token;
        }

        public HttpClient GetClient(string baseUrl)
        {
            var client = new HttpClient(new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new System.Uri(baseUrl),
            };
            return client;
        }
    }
}
