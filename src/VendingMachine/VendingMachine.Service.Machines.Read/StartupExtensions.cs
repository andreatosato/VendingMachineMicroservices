using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines
{
    public static partial class StartupExtensions
    {
        public static IServiceCollection AddMachineQueries(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IMachineQuery>(t => new MachinesQuery(connectionString));
            return services;
        }
    }
}
