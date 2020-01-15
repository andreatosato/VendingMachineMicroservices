using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Application.Cachings;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Infrastructure;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Read;
using VendingMachine.Service.Shared.Exceptions;

namespace VendingMachine.Service.Machines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class MachineItemsController : ControllerBase
    {
        private readonly IMachineQuery machineQuery;
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger logger;

        public MachineItemsController(IMachineQuery machineQuery, IMachinesUoW machinesUoW, IMediator mediator, IDistributedCache distributedCache, ILoggerFactory loggerFactory)
        {
            this.machineQuery = machineQuery;
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.logger = loggerFactory.CreateLogger(typeof(MachineItemsController));
        }

        [HttpGet("{machineId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfosAsync([FromRoute] int machineId)
        {
            if (machineId > 0)
            {
                var cache = await (await distributedCache.GetAsync(CachingKeys.MachineInformationKey(machineId)))
                    .DeserializeCacheAsync<Read.Models.MachineItemReadModel>();

                if (cache != null)
                    return Ok(cache);

                var result = await machineQuery.GetMachineItemInfoAsync(machineId);
                await distributedCache.SetAsync(
                    CachingKeys.MachineInformationKey(machineId),
                    result.SerializeCache(),
                    DistribuitedCacheOptions.SlideStandard);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCreateMachineItemAsync([FromBody] CreateMachineItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var machineId = await mediator.Send(new CreateNewMachineCommand()
                {
                    Status = model.Status,
                    X = model.Position?.X,
                    Y = model.Position?.Y,
                    Temperature = model.Temperature,
                    Model = model.Model.ModelName,
                    Version = model.Model.Version
                }).ConfigureAwait(false);
                return Created($"{machineId}/GetInfos", model);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{machineId:int}")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMachineAsync([FromRoute] int machineId)
        {
            if (machineId > 0)
            {
                try
                {
                    await mediator.Send(new DeleteMachineCommand() { MachineId = machineId });
                }
                catch (NotExistsException nex)
                {
                    logger.LogError(nex, "Not found machine");
                    return NotFound();
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{machineId:int}/Coins")]
        public async Task<IActionResult> GetCoinsAsync(int machineId)
        {
            Read.Models.CoinsInMachineReadModel coins = await machineQuery.GetCoinsInMachineAsync(machineId).ConfigureAwait(false);
            return Ok(coins);
        }

        [HttpGet("{machineId:int}/ActiveProducts")]
        [ProducesResponseType(typeof(Read.Models.ProductsReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveProductsAsync(int machineId)
        {
            if(machineId > 0)
            {
                Read.Models.ProductsReadModel products = await machineQuery.GetProductsInMachineAsync(machineId).ConfigureAwait(false);
                return Ok(products);
            }
            return BadRequest("MachineId is not correct");
        }

        [HttpGet("{machineId:int}/HistoryProducts")]
        [ProducesResponseType(typeof(Read.Models.HistoryProductsReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHistoryProductsAsync(int machineId)
        {
            if (machineId > 0)
            {
                Read.Models.HistoryProductsReadModel products = await machineQuery.GetHistoryProductsInMachineAsync(machineId).ConfigureAwait(false);
                return Ok(products);
            }
            return BadRequest("MachineId is not correct");
        }

        [HttpPost("{machineId:int}/BuyProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostBuyProductsAsync([FromBody] BuyProductsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new BuyProductsMachineCommand()
                {
                    MachineId = model.MachineId,
                    ProductsBuy = model.Products,
                    TotalBuy = model.TotalBuy,
                    TotalRest = model.TotalRest
                }).ConfigureAwait(false);
                return Ok();
            }
            logger.LogDebug("Error in input data for BuyProducts {@model}", model);
            return BadRequest(ModelState);
        }


        [HttpPost("{machineId:int}/LoadProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLoadProductsAsync([FromBody] LoadProductsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new LoadProductsMachineCommand()
                {
                    MachineId = model.MachineId,
                    Products = model.Products
                }).ConfigureAwait(false);

                return Ok();
            }
            logger.LogDebug("Error in input data for LoadProducts {@model}", model);
            return BadRequest(ModelState);
        }


        [HttpPut("{machineId:int}/SetTemperature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetTemperatureAsync([FromRoute] int machineId, [FromBody] SetTemperatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new SetTemperatureMachineCommand() { MachineId = machineId, Data = model.Data} );
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{machineId:int}/SetStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetStatusAsync([FromRoute] int machineId, [FromBody] SetStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new SetStatusMachineCommand() { MachineId = machineId, Data = model.Data });
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{machineId:int}/SetPosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetPositionAsync([FromRoute] int machineId, [FromBody] SetPositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mediator.Send(new SetPositionMachineCommand() { MachineId = machineId, Data = new MapPointModel(model.Data.X, model.Data.Y ) });
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}