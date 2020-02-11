using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Threading.Tasks;
using VendingMachine.Service.Gateway.RefitModels;
using VendingMachine.Service.Gateway.RefitModels.Auth;
using VendingMachine.UI.Authentication;

namespace VendingMachine.UI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRefitClients(this IServiceCollection services)
        {
            services.AddTransient<IAuthUserClient, AutenticatedUserHttpClient>();
            services.AddTransient(sp => RestService.For<IAuthenticationApi>(sp.GetRequiredService<ServicesReference>().AuthService));
            services.AddTransient<IGatewayApi>((sp) =>
            {
                IGatewayApi r = null;
                Task.Factory.StartNew(async () =>
                {
                    string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
                    var accessTokenStore = sp.GetRequiredService<IAccessTokenReader>();
                    string token = await accessTokenStore.GetTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, token);
                        r = RestService.For<IGatewayApi>(httpClient);
                        return r;
                    }
                    else
                        throw new ArgumentNullException("Access Token is null");
                }).Wait();
                return r;
            });
            return services;
        }

        
    }

    public static class RestClient
    {
        public static async Task<IGatewayApi> CreateUserClient(this IServiceProvider sp)
        {
            string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
            var accessTokenStore = sp.GetRequiredService<IAccessTokenReader>();
            string token = await accessTokenStore.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, token);
                var r = RestService.For<IGatewayApi>(httpClient);
                return r;
            }
            else
            {
                throw new ArgumentNullException("Access Token is null");
            }
        }
    }
}
