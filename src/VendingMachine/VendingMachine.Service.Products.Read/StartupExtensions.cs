using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products
{
    public static partial class StartupExtensions
    {
        public static IServiceCollection AddProductQueries(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IProductQuery>(t => new ProductQuery(connectionString));
            services.AddTransient<IProductItemQuery>(t => new ProductItemQuery(connectionString));
            return services;
        }
    }
}
