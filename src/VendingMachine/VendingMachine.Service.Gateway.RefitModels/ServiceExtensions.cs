using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using VendingMachine.Service.Gateway.RefitModels.Auth;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRefitClients(this IServiceCollection services)
        {
            services.AddTransient<IAuthClient, AutenticatedHttpClient>();
            services.AddTransient(sp => RestService.For<IAuthenticationApi>(sp.GetRequiredService<ServicesReference>().AuthService));
            services.AddTransient(sp => {
                try
                {
                    string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
                    var httpClient = sp.GetRequiredService<IAuthClient>().GetClient(url);
                    var r = RestService.For<IGatewayApi>(httpClient);
                    return r;
                }
                catch (Exception ex)
                {

                    throw;
                }
            });
            //services.AddTransient(serviceProvider => GetAutenticationRestService<IAuthenticationApi>(serviceProvider));
            //services.AddTransient(serviceProvider => GetGatewayRestService<IGatewayApi>(serviceProvider));
            return services;
        }

        private static T GetRestService<T>(IServiceProvider serviceProvider, string Url)
        {
            try
            {
                var httpClient = serviceProvider.GetRequiredService<IAuthClient>().GetClient(Url);
                T result = RestService.For<T>(httpClient);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static T GetAutenticationRestService<T>(IServiceProvider serviceProvider)
        {
            return GetRestService<T>(serviceProvider, serviceProvider.GetRequiredService<ServicesReference>().AuthService);
        }

        private static T GetGatewayRestService<T>(IServiceProvider serviceProvider)
        {
            return GetRestService<T>(serviceProvider, serviceProvider.GetRequiredService<ServicesReference>().GatewayBackendService);
        }
    }
}
