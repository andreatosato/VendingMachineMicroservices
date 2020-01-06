using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Infrastructure.Commands;

namespace VendingMachine.Service.Machines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // POST: api/Coins
        [HttpPost(Name = "Add")]
        public async Task<IActionResult> Post([FromBody] AddCoinsViewModel model)
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
    }
}
