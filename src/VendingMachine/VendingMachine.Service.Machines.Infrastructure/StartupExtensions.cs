using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Machines.Infrastructure;
using VendingMachine.Service.Machines.Infrastructure.Repositories;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines
{
    public static partial class StartupExtensions
    {
        public static IServiceCollection AddMachineInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IRepository<MachineItem>, MachineItemRepository>();
            services.AddTransient<IRepository<MachineType>, MachineTypeRepository>();
            services.AddTransient<IMachinesUoW, MachinesUoW>();
            return services;
        }

        public static void AddMachineEntityFrameworkDev(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MachineContext>(opt =>
            {
                opt.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
                opt.EnableSensitiveDataLogging();
            });
        }

        public static void AddMachineEntityFrameworkProd(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MachineContext>(opt =>
            {
                opt.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
            });
        }
    }
}
