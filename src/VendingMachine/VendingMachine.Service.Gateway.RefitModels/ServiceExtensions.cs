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
                string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
                var httpClient = sp.GetRequiredService<IAuthClient>().GetClient(url);
                var r = RestService.For<IGatewayApi>(httpClient);
                return r;
            });
            return services;
        }
    }
}
