using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Service.Products.Application.ViewModels.ProductItems;
using VendingMachine.Service.Products.Infrastructure.Commands;
using VendingMachine.Service.Products.Read.Models;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("ProductItems")]
    [ApiController]
    public class ProductItemsV1Controller : ControllerBase
    {
        private readonly IProductItemQuery query;
        private readonly IMediator mediator;

        public ProductItemsV1Controller(IProductItemQuery query, IMediator mediator)
        {
            this.query = query;
            this.mediator = mediator;
        }

        [HttpGet("{productItemId:int}")]
        [ProducesResponseType(typeof(ProductItemReadModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfosAsync([FromRoute] int productItemId)
        {
            if (productItemId > 0)
            {
                if (!await query.ExistProductItemAsync(productItemId))
                {
                    return NotFound();
                }

                var result = await query.GetProductInfoAsync(productItemId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCreateProductItemAsync([FromBody] ProductItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(new ProductItemAddCommand()
                {
                    ProductId = model.ProductId,
                    ExpirationDate = model.ExpirationDate.GetValueOrDefault(),
                    SoldPrice = model.SoldPrice.GetValueOrDefault(),
                    Purchased = model.Purchased.GetValueOrDefault()
                });
                return Created($"{response.ProductItemId}", response);
                //return CreatedAtAction(nameof(GetInfosAsync), new { productItemId = response.ProductItemId }, response);
            }
            return BadRequest(ModelState);
        }
    }
}