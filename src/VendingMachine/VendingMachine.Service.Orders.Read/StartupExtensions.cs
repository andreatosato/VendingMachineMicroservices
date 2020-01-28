using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Orders.Read.Queries;

namespace VendingMachine.Service.Orders.Read
{
    public static partial class StartupExtensions
    {
        public static IServiceCollection AddOrderQueries(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IOrderQuery>(t => new OrderQuery(connectionString));
            return services;
        }
    }
}
