# Worker
Il worker consente di processare in background informazioni.
Potrebbe essere utilizzato per:
1. Leggere file (questo particolare esempio)
2. Rimanere in ascolto di messaggi ed elaborarli
3. Rimanere in ascolto di flussi informativi
4. Il peggior (polling continuo)

```cs
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public IServiceProvider Services { get; }
    private WatcherConfiguration configuration;
    private FileSystemWatcher watcherProduct;
    private FileSystemWatcher watcherProductItem;

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
        watcherProductItem = new FileSystemWatcher
        {
            Path = Path.GetTempPath() + configuration.ProductItemPath,
            EnableRaisingEvents = true,
            IncludeSubdirectories = false
        };
        watcherProductItem.Created += WatcherProductItem_Created;

        return base.StartAsync(cancellationToken);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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
        }
        
    }

}
```
