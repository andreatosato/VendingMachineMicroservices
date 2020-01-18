using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Products.Data;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Products.Infrastructure;
using VendingMachine.Service.Products.Infrastructure.Repositories;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddProductInfrastructure(this IServiceCollection services)
        {
            services.AddMapper();
            services.AddTransient<IRepository<Product>, ProductRepository>();
            services.AddTransient<IRepository<ProductItem>, ProductItemRepository>();
            services.AddTransient<IProductsUoW, ProductsUoW>();
            return services;
        }

        public static void AddProductEntityFrameworkDev(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ProductContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
                opt.EnableSensitiveDataLogging();
            });
        }

        public static void AddProductEntityFrameworkProd(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ProductContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
        }
    }
}
