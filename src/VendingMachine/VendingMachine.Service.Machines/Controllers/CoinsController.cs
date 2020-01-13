using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Infrastructure.Commands;

namespace VendingMachine.Service.Machines.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class CoinsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public CoinsController(IMediator mediator, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.logger = loggerFactory.CreateLogger<CoinsController>();
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] AddCoinsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await mediator.Send(new AddCoinsMachineCommand()
                    {
                        CoinsAdded = model.Coins,
                        MachineId = model.MachineId
                    });
                    logger.LogInformation("Coins Added: {@Coins} in machine: {@MachineId}", model.Coins, model.MachineId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError("AddCoins", ex);
                    throw;
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Collect")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CollectCoinsMachineResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Collect([FromBody] CollectCoinsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await mediator.Send(new CollectCoinsMachineCommand()
                    {
                        MachineId = model.MachineId
                    });
                    logger.LogInformation("Coins Collected from machine: {@MachineId}, collected: {CoinsCollected}",  model.MachineId, result.CoinsCollected);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError("Collect Coins", ex);
                    throw;
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("RequestRest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseRestErrorViewModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostRequestRestAsync([FromBody] RequestRestViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal restInMachine = await mediator.Send(new RequestRestMachineCommand() { MachineId = model.MachineId, Rest = model.Rest });

                if (restInMachine == model.Rest)
                    return Ok();
                decimal restCalculated = restInMachine - model.Rest;
                if (restCalculated > 0)
                    return StatusCode((int)HttpStatusCode.InternalServerError,
                        new ResponseRestErrorViewModel() { Difference = restCalculated, ErrorType = RestErrorType.MoreCoinsInMachine });
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError,
                        new ResponseRestErrorViewModel() { Difference = restCalculated, ErrorType = RestErrorType.LessCoinsInMachine });
            }
            return BadRequest(ModelState);
        }
    }
}
