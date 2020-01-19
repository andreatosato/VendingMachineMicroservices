using Microsoft.Extensions.Caching.Distributed;
using System;

namespace VendingMachine.Service.Products.Application.Caching
{
    public static class DistribuitedCacheOptions
    {
        public static DistributedCacheEntryOptions SlideStandard => new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(15),
        };
    }
}
