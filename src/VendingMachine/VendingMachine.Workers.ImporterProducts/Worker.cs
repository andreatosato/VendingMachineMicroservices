using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VendingMachine.Workers.ImporterProducts.Readers;

namespace VendingMachine.Workers.ImporterProducts
{
    // https://docs.microsoft.com/it-it/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IServiceProvider Services { get; }
        private WatcherConfiguration configuration;
        private FileSystemWatcher watcherProduct;
        private FileSystemWatcher watcherProductItem;
        private bool productIsWorking = false;

        public Worker(ILogger<Worker> logger, IServiceProvider services, WatcherConfiguration configuration)
        {
            _logger = logger;
            Services = services;
            this.configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            watcherProduct = new FileSystemWatcher
            {
                Path = Path.GetTempPath() + configuration.ProductPath,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };
            watcherProduct.Created += WatcherProduct_Created;
            //watcherProduct.Changed += WatcherProduct_Created;

            watcherProductItem = new FileSystemWatcher
            {
                Path = Path.GetTempPath() + configuration.ProductItemPath,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };
            watcherProductItem.Created += WatcherProductItem_Created;
            //watcherProductItem.Changed += WatcherProductItem_Created;

            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void WatcherProductItem_Created(object sender, FileSystemEventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                using (var scope = Services.CreateScope())
                {
                    var productItemsImporter = scope.ServiceProvider.GetRequiredService<IProductItemsImporter>();
                    await productItemsImporter.DoWorkAsync(e.Name, e.FullPath);
                }
            }).Wait();           
        }

        private void WatcherProduct_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                productIsWorking = true;
                Task.Factory.StartNew(async () =>
                {
                    using (var scope = Services.CreateScope())
                    {
                        var productImporter = scope.ServiceProvider.GetRequiredService<IProductImporter>();
                        await productImporter.DoWorkAsync(e.Name, e.FullPath);
                    }
                }).Wait();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                productIsWorking = false;
            }
           
        }

    }
}
