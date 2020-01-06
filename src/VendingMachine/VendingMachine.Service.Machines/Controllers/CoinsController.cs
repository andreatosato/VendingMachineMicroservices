using System;
using System.Collections.Generic;
using System.Linq;
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

        //// GET: api/Coins
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Coins/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        //// PUT: api/Coins/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [HttpPost("Add")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CollectCoinsMachineResult), (int)HttpStatusCode.OK)]
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
    }
}
