using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using VendingMachine.Service.Gateway.RefitModels;
using VendingMachine.Service.Gateway.RefitModels.Auth;

namespace VendingMachine.UI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRefitClients(this IServiceCollection services)
        {
            services.AddTransient<IAuthUserClient, AutenticatedUserHttpClient>();
            services.AddTransient(sp => RestService.For<IAuthenticationApi>(sp.GetRequiredService<ServicesReference>().AuthService));
            return services;
        }

        
    }

    public static class RestClient
    {
        public static IGatewayApi CreateUserClient(this IServiceProvider sp, string username, string password)
        {
            string url = sp.GetRequiredService<ServicesReference>().GatewayBackendService;
            var httpClient = sp.GetRequiredService<IAuthUserClient>().GetClient(url, username, password);
            var r = RestService.For<IGatewayApi>(httpClient);
            return r;
        }
    }
}
