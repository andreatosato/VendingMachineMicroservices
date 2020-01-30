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
        public WatcherConfiguration configuration;

        public Worker(ILogger<Worker> logger, IServiceProvider services, WatcherConfiguration configuration)
        {
            _logger = logger;
            Services = services;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = configuration.Path;
                    watcher.Created += Watcher_Created;
                }
                //await Task.Delay(1000, stoppingToken);
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
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

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            
        }
    }
}
