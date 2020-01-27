using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Products.ServiceCommunications.Client.Services;

namespace VendingMachine.Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddProductClient(this IServiceCollection services)
        {
            services.AddTransient<IProductItemClientService, ProductItemClientService>();
            services.AddTransient<IProductClientService, ProductClientService>();
            return services;
        }
    }
}
