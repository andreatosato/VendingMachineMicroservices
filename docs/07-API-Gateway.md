# Ocelot
[Ocelot](https://github.com/ThreeMammals/Ocelot) consente di raggruppare sotto una unica API tutte le API sviluppate a microservizi.

```json
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/connect/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 44330
        }
      ],
      "DownstreamHttpVersion": "2.0",
      "UpstreamPathTemplate": "/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "Key": "Auth.Api",
      "DangerousAcceptAnyServerCertificateValidator": true
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 4020
        }
      ],
      "DownstreamHttpVersion": "2.0",
      "UpstreamPathTemplate": "/product-api/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "Key": "Product.Api",
      "DangerousAcceptAnyServerCertificateValidator": true
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 4010
        }
      ],
      "DownstreamHttpVersion": "2.0",
      "UpstreamPathTemplate": "/machine-api/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "Key": "Machine.Api",
      "DangerousAcceptAnyServerCertificateValidator": true
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 4030
        }
      ],
      "UpstreamPathTemplate": "/order-api/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "Key": "Order.Api",
      "DangerousAcceptAnyServerCertificateValidator": true
    },
    {
      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 4040
        }
      ],
      "DownstreamHttpVersion": "2.0",
      "UpstreamPathTemplate": "/aggregator-api/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "Key": "Aggregator.Api",
      "DangerousAcceptAnyServerCertificateValidator": true
    }
  ],
  "Aggregates": [

  ],
  "GlobalConfiguration": {

  }
}
```

Non sempre un API gateway è la soluzione corretta, a volte, è necessario creare dei strati intermedi che aiutino a gestire le logiche di più API.
Questo strato si chiama Aggregator.

E' stato implementato nel progetto: **VendingMachine.Service.Aggregators.Web.API**

In particolare, questo servizio, non dialoga direttamente via REST, ma bensì va a comunicare con le API con un metodo più veloce e persistente. In questo caso gRPC.
L'alternativa è il dialogo via Messaggi.
```cs
[ApiController]
[Route("[controller]")]
public class AggregationMachineController : ControllerBase
{
    private readonly ProductItems.ProductItemsClient productItemsClient;
    private readonly IMachineClientService machineClient;

    public AggregationMachineController(ProductItems.ProductItemsClient productItemsClient,
        IMachineClientService machineClient)
    {
        this.productItemsClient = productItemsClient;
        this.machineClient = machineClient;
    }

    [HttpGet("{machineId:int}")]
    public async Task<IActionResult> GetMachineCurrentStatusAsync(int machineId)
    {
        var machineExist = await machineClient.ExistMachineAsync(machineId);
        if (machineExist)
        {
            var machineInfos = await machineClient.GetMachineInfoAsync(machineId);
            var productIds = machineInfos.Machine.ActiveProducts.Select(x => x.Id).ToList();

            List<ProductItemsServiceModel> products = new List<ProductItemsServiceModel>();
            GetProductItemsRequest productItemsRequest = new GetProductItemsRequest();
            productItemsRequest.ProductIds.AddRange(productIds);
            using (var productStream = productItemsClient.GetProductItems(productItemsRequest))
            {
                while (await productStream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    ProductItemsServiceModel product = productStream.ResponseStream.Current;
                    products.Add(product);
                }
            }

            return Ok(MachineItemViewModels.ToViewModel(machineInfos.Machine, products));
        }
        return BadRequest("MachineItem not found");
    }
}
```

Ecco un esempio più strutturato dell'utilizzo di Api gateway:

![eShopOnContainers](https://docs.microsoft.com/it-it/dotnet/architecture/microservices/multi-container-microservice-net-applications/media/implement-api-gateways-with-ocelot/eshoponcontainers-architecture-aggregator-services.png)

[Maggiori informazioni](https://docs.microsoft.com/it-it/dotnet/architecture/microservices/multi-container-microservice-net-applications/implement-api-gateways-with-ocelot)