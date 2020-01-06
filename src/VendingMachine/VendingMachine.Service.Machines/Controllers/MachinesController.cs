using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineQuery machineQuery;

        public MachinesController(IMachineQuery machineQuery)
        {
            this.machineQuery = machineQuery;
        }

        [HttpGet("/{machineId}/Coins")]
        public async Task<IActionResult> GetCoinsAsync(int machineId)
        {
            Read.Models.CoinsInMachineReadModel coins = await machineQuery.GetCoinsInMachineAsync(machineId).ConfigureAwait(false);
            return Ok(coins);
        }
    }
}