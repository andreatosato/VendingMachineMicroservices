using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Orders.Data;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Orders.Infrastructure.Repositories;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddOrderInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Order>, OrderRepository>();
            services.AddTransient<IOrdersUoW, OrdersUoW>();
            return services;
        }

        public static void AddOrderEntityFrameworkDev(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<OrderContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
                opt.EnableSensitiveDataLogging();
            });
        }

        public static void AddOrderEntityFrameworkProd(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<OrderContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
        }
    }
}
