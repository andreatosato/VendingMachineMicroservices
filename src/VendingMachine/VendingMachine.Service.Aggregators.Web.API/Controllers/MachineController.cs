using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Products.ServiceCommunications;

namespace VendingMachine.Service.Aggregators.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly ProductItems.ProductItemsClient productItemsClient;

        public MachineController(ProductItems.ProductItemsClient productItemsClient)
        {
            this.productItemsClient = productItemsClient;
        }

        [HttpGet("{machineId:int}")]
        public async Task<IActionResult> GetMachineAsync(int machineId)
        {
            //TODO: request Machine Id 
            //machineId
            // Read Payload

            //Then, foreach product...
            var request = new ProductsRequest();
            request.ProductIds.Add(new int[1] { 1 });
            while (await productItemsClient.GetProducts(request).ResponseStream.MoveNext(CancellationToken.None))
            {
                var product = productItemsClient.GetProducts(request).ResponseStream.Current;
            }

            throw new NotImplementedException();
        }
    }
}
