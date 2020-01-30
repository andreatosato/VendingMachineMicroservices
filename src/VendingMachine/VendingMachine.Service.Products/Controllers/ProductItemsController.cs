using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ProductItems")]
    [ApiController]
    public class ProductItemsV1Controller : ControllerBase
    {
        private readonly IProductItemQuery query;

        public ProductItemsV1Controller(IProductItemQuery query)
        {
            this.query = query;
        }

        [HttpGet("{productItemId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfosAsync([FromRoute] int productItemId)
        {
            if(productItemId > 0)
            {
                if(!await query.ExistProductItemAsync(productItemId))
                {
                    return NotFound();
                }

                var result = await query.GetProductInfoAsync(productItemId);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}