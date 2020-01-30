using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VendingMachine.Service.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ProductItems")]
    [ApiController]
    public class ProductItemsV1Controller : ControllerBase
    {
        //TODO: NotImplemented
        [HttpGet("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfosAsync([FromRoute] int productId)
        {
            throw new NotImplementedException();
        }
    }
}