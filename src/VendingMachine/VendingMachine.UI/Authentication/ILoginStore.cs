using Blazor.Extensions.Storage.Interfaces;
using System.Threading.Tasks;

namespace VendingMachine.UI.Authentication
{
    public interface ILoginStore
    {
        string Username { get; set; }
        string Password { get; set; }
    }

    public class LoginStore : ILoginStore
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AccessTokenReader : IAccessTokenReader
    {
        private const string AccessTokenKey = nameof(AccessTokenKey);
        private ILocalStorage localStorage;
        private string token; // TODO

        public AccessTokenReader(ILocalStorage localStorage)
        {
            this.localStorage = localStorage;
            Task.Run(async () =>
            {
                this.token = await this.GetTokenAsync();
            }).ConfigureAwait(false);
        }

        public async Task<string> GetTokenAsync()
        {
            return await localStorage.GetItem<string>(AccessTokenKey).ConfigureAwait(false);
        }

        public string GetToken()
        {
            return token;
        }

        public async Task SetTokenAsync(string accessToken)
        {
            await localStorage.SetItem<string>(AccessTokenKey, accessToken).ConfigureAwait(false);
            token = accessToken;
        }
    }

    public interface IAccessTokenReader
    {
        Task SetTokenAsync(string accessToken);
        Task<string> GetTokenAsync();
        string GetToken();
    }
}
