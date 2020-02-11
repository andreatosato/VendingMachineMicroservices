using Blazor.Extensions.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class AccessTokenStore : IAccessTokenReader
    {
        private const string AccessTokenKey = nameof(AccessTokenKey);
        private ISessionStorage sessionStorage;
        public AccessTokenStore(ISessionStorage sessionStorage)
        {
            this.sessionStorage = sessionStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            return await sessionStorage.GetItem<string>(AccessTokenKey);
        }

        public async Task SetTokenAsync(string accessToken)
        {
            await sessionStorage.SetItem<string>(AccessTokenKey, accessToken);
        }
    }

    public interface IAccessTokenReader
    {
        Task SetTokenAsync(string accessToken);
        Task<string> GetTokenAsync();
    }
}
