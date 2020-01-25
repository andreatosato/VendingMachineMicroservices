using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Read
{
    public static partial class StartupExtensions
    {
        public static IServiceCollection AddOrderQueries(this IServiceCollection services, string connectionString)
        {
            //services.AddTransient<IProductQuery>(t => new ProductQuery(connectionString));
            //services.AddTransient<IProductItemQuery>(t => new ProductItemQuery(connectionString));
            return services;
        }
    }
}
