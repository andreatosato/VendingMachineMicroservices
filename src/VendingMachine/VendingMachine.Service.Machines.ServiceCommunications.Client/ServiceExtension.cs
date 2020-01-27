using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;

namespace VendingMachine.Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMachineClient(this IServiceCollection services)
        {
            services.AddTransient<IMachineClientService, MachineClientService>();
            return services;
        }
    }
}
