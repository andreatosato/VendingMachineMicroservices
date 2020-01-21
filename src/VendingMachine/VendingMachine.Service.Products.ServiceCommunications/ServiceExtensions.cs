using Microsoft.Extensions.DependencyInjection;

namespace VendingMachine.Service.Products.ServiceCommunications
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceCommunications(this IServiceCollection services)
        {
            return services;
        }
    }
}
