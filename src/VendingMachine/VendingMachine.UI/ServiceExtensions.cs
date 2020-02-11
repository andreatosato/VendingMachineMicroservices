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
                string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
                var accessTokenStore = sp.GetRequiredService<IAccessTokenReader>();
                string token = accessTokenStore.GetTokenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(token))
                {
                    var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, token);
                    var r = RestService.For<IGatewayApi>(httpClient);
                    return r;
                }
                else
                    throw new ArgumentNullException("Access Token is null");                
            });
            return services;
        }
    }
}
