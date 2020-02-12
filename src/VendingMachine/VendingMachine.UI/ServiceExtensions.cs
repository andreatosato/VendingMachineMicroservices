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
            services.AddTransient<IAuthUserClient, AutenticatedBlazorUserHttpClient>();
            services.AddTransient(sp => RestService.For<IAuthenticationApi>(sp.GetRequiredService<ServicesReference>().AuthService));
            services.AddScoped<IGatewayApiBlazor>((sp) =>
            {
                IGatewayApiBlazor r = null;
                string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
                var accessTokenStore = sp.GetRequiredService<IAccessTokenReader>();
                try
                {
                    string token = accessTokenStore.GetToken();                   
                    if (!string.IsNullOrEmpty(token))
                    {
                        var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, token);
                        r = RestService.For<IGatewayApiBlazor>(httpClient);
                        Console.WriteLine("return internal api");
                        return r;
                    }
                    else
                        throw new ArgumentNullException("Access Token is null");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                //string token = accessTokenStore.GetTokenAsync().Result;
                //Console.WriteLine("token " + token);
                //if (!string.IsNullOrEmpty(token))
                //{
                //    var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, token);
                //    var r = RestService.For<IGatewayApi>(httpClient);
                //    return r;
                //}
                //else
                //    throw new ArgumentNullException("Access Token is null");                
            });
            return services;
        }
    }
}
