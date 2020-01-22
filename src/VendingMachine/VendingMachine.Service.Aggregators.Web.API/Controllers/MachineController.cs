using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.ServiceCommunications;
using VendingMachine.Service.Products.ServiceCommunications;

namespace VendingMachine.Service.Aggregators.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly ProductItems.ProductItemsClient productItemsClient;
        private readonly MachineItems.MachineItemsClient machineItemsClient;

        public MachineController(ProductItems.ProductItemsClient productItemsClient, 
            MachineItems.MachineItemsClient machineItemsClient)
        {
            this.productItemsClient = productItemsClient;
            this.machineItemsClient = machineItemsClient;
        }

        [HttpGet("{machineId:int}")]
        public async Task<IActionResult> GetMachineAsync(int machineId)
        {
            //TODO: request Machine Id 
            //machineId
            // Read Payload

            //Then, foreach product...
            var request = new GetProductItemsRequest();
            request.ProductIds.Add(new int[1] { 1 });
            while (await productItemsClient.GetProductItems(request).ResponseStream.MoveNext(CancellationToken.None))
            {
                var product = productItemsClient.GetProductItems(request).ResponseStream.Current;
            }

            throw new NotImplementedException();
        }
    }
}
