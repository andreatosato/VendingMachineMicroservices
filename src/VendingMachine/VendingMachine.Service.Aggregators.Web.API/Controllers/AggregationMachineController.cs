using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Aggregators.Web.API.ViewModels.Machine;
using VendingMachine.Service.Machines.ServiceCommunications;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;
using VendingMachine.Service.Products.ServiceCommunications;

namespace VendingMachine.Service.Aggregators.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
}
