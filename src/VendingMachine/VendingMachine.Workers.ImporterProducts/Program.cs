using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VendingMachine.Service.Gateway.RefitModels;
using VendingMachine.Service.Gateway.RefitModels.Auth;
using VendingMachine.Workers.ImporterProducts.Readers;

namespace VendingMachine.Workers.ImporterProducts
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
          .AddEnvironmentVariables()
          .Build();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    var watcherConfiguration = new WatcherConfiguration();
                    Configuration.GetSection("WatcherConfiguration").Bind(watcherConfiguration);
                    services.AddSingleton<WatcherConfiguration>(watcherConfiguration);

                    var serviceReference = new ServicesReference();
                    Configuration.Bind(nameof(ServicesReference), serviceReference);
                    services.AddSingleton<ServicesReference>(serviceReference);

                    services.AddTransient<IProductItemsImporter, ProductItemsImporter>();
                    services.AddTransient<IProductImporter, ProductImporter>();
                    services.AddRefitClients();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
