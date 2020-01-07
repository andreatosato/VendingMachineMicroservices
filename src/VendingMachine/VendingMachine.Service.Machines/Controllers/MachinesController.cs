using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Infrastructure;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineQuery machineQuery;
        private readonly IMachinesUoW machinesUoW;
        private readonly ILogger logger;

        public MachinesController(IMachineQuery machineQuery, IMachinesUoW machinesUoW, ILoggerFactory loggerFactory)
        {
            this.machineQuery = machineQuery;
            this.machinesUoW = machinesUoW;
            this.logger = loggerFactory.CreateLogger(typeof(MachinesController));
        }

        [HttpGet("{machineId}/Coins")]
        public async Task<IActionResult> GetCoinsAsync(int machineId)
        {
            Read.Models.CoinsInMachineReadModel coins = await machineQuery.GetCoinsInMachineAsync(machineId).ConfigureAwait(false);
            return Ok(coins);
        }

        [HttpGet("{machineId}/ActiveProducts")]
        [ProducesResponseType(typeof(Read.Models.ProductsReadModel), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetActiveProductsAsync(int machineId)
        {
            if(machineId > 0)
            {
                Read.Models.ProductsReadModel products = await machineQuery.GetProductsInMachineAsync(machineId).ConfigureAwait(false);
                return Ok(products);
            }
            return BadRequest("MachineId is not correct");
        }

        [HttpPost("{machineId}/BuyProducts")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostBuyProductsAsync([FromQuery] int machineId, [FromBody] BuyProductsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var machine = await machinesUoW.MachineRepository.FindAsync(machineId).ConfigureAwait(false);
                List<Domain.Product> productsToBuy = new List<Domain.Product>();
                foreach (var p in model.Products)
                {
                    var productActive = machine.ActiveProducts.Find(t => t.Id == p);
                    if (productActive == null)
                        throw new ArgumentException($"Products [{p}] is not available");
                    productsToBuy.Add(productActive);
                }

                machine.BuyProducts(productsToBuy);
                // Subtract products cost to coins inserted.
                machine.SupplyCoins(model.TotalBuy);
                // Check rest in machine
                if (machine.CoinsCurrentSupply != model.TotalRest)
                    throw new InvalidOperationException("Coins in machine isn't equals to coins to rest.");
                
                await machinesUoW.SaveAsync().ConfigureAwait(false);
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}