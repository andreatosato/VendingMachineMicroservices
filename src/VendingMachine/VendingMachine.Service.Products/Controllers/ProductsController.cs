using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VendingMachine.Service.Products.Application.Caching;
using VendingMachine.Service.Products.Infrastructure;
using VendingMachine.Service.Products.Read.Models;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductQuery productQuery;
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger logger;

        public ProductsController(IProductQuery productQuery, IMediator mediator, IDistributedCache distributedCache, ILoggerFactory loggerFactory)
        {
            this.productQuery = productQuery;
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.logger = loggerFactory.CreateLogger(typeof(ProductsController));
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
    }
}