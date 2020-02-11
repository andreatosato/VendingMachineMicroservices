using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VendingMachine.Service.Products.Application.Caching;
using VendingMachine.Service.Products.Application.ViewModels.Products;
using VendingMachine.Service.Products.Infrastructure.Commands;
using VendingMachine.Service.Products.Read.Models;
using VendingMachine.Service.Products.Read.Queries;
using VendingMachine.Service.Shared.Exceptions;

namespace VendingMachine.Service.Products.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("Products")]
    [ApiController]
    public class Productsv1Controller : ControllerBase
    {
        [HttpGet("{productId:int}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult IgnoredApi([FromRoute] int productId)
        {
            return Ok();
        }

        [HttpGet("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult NotIgnoredApi([FromRoute] int productId)
        {
            return Ok();
        }
    }

    [ApiVersion("2.0")]    
    [Route("Products")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IProductQuery productQuery;
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger logger;

        public ProductsV2Controller(IProductQuery productQuery, IMediator mediator, IDistributedCache distributedCache, ILoggerFactory loggerFactory)
        {
            this.productQuery = productQuery;
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.logger = loggerFactory.CreateLogger(typeof(ProductsV2Controller));
        }

        [HttpGet("{productId:int}")]
        [ProducesResponseType(typeof(ColdDrinkReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HotDrinkReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SnackReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfosAsync([FromRoute] int productId)
        {
            if (productId > 0)
            {
                var cacheData = await distributedCache.GetAsync(CachingKeys.ProductInformationKey(productId));
                if(cacheData != null)
                {
                    var baseType = await cacheData.DeserializeCacheAsync<ProductReadModel>();
                    ProductReadModel cache = null;
                    switch (baseType.NameOf)
                    {
                        case nameof(ColdDrinkReadModel):
                            cache = await cacheData.DeserializeCacheAsync<ColdDrinkReadModel>();
                            break;
                        case nameof(HotDrinkReadModel):
                            cache = await cacheData.DeserializeCacheAsync<HotDrinkReadModel>();
                            break;
                        case nameof(SnackReadModel):
                            cache = await cacheData.DeserializeCacheAsync<SnackReadModel>();
                            break;
                    }
                    return Ok(cache);
                }

                var result = await productQuery.GetProductInfoAsync(productId);
                await distributedCache.SetAsync(
                    CachingKeys.ProductInformationKey(productId),
                    result.SerializeCache(),
                    DistribuitedCacheOptions.SlideStandard);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("List")]
        [ProducesResponseType(typeof(ColdDrinkReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HotDrinkReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SnackReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsAsync([FromBody] List<int> productIds)
        {
            if (!(!productIds.Any() && productIds.Any(x => x <= 0)))
            {
                
                var result = await productQuery.GetProductsInfoAsync(productIds);                
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Snack")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCreateSnackAsync([FromBody] SnackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productId = await mediator.Send(new SnackAddCommand()
                {
                    Name = model.Name,
                    Price = new PriceCommand()
                    {
                        GrossPrice = model.Price.GrossPrice,
                        TaxPercentage = model.Price.TaxPercentage
                    },
                    Grams = model.Grams
                }).ConfigureAwait(false);
                model.Id = productId.Id;
                return Created($"{model.Id}", model);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("ColdDrink")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCreateColdDrinkAsync([FromBody] ColdDrinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productId = await mediator.Send(new ColdDrinkAddCommand()
                {
                    Name = model.Name,
                    Price = new PriceCommand()
                    {
                        GrossPrice = model.Price.GrossPrice,
                        TaxPercentage = model.Price.TaxPercentage
                    },
                    TemperatureMaximum = model.TemperatureMaximum,
                    TemperatureMinimum = model.TemperatureMinimum
                }).ConfigureAwait(false);
                model.Id = productId.Id;
                return Created($"{model.Id}", model);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("HotDrink")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCreateHotDrinkAsync([FromBody] HotDrinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productId = await mediator.Send(new HotDrinkAddCommand()
                {
                    Name = model.Name,
                    Price = new PriceCommand()
                    {
                        GrossPrice = model.Price.GrossPrice,
                        TaxPercentage = model.Price.TaxPercentage
                    },
                    TemperatureMaximum = model.TemperatureMaximum,
                    TemperatureMinimum = model.TemperatureMinimum
                }).ConfigureAwait(false);
                model.Id = productId.Id;
                return Created($"{model.Id}", model);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{productId:int}")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int productId)
        {
            if (productId > 0)
            {
                try
                {
                    await mediator.Send(new ProductDeleteCommand() { ProductId = productId });
                }
                catch (NotExistsException nex)
                {
                    logger.LogError(nex, "Not found product");
                    return NotFound();
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}