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
            services.AddTransient(serviceProvider => GetAutenticationRestService<IAuthenticationApi>(serviceProvider));
            services.AddTransient(serviceProvider => GetGatewayRestService<IGatewayApi>(serviceProvider));
            return services;
        }

        private static T GetRestService<T>(IServiceProvider serviceProvider, string Url)
        {
            return RestService.For<T>(serviceProvider.GetRequiredService<IAuthClient>().GetClient(Url));
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
