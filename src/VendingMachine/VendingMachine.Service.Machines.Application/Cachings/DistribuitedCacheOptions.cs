using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.Cachings
{
    public static class DistribuitedCacheOptions
    {
        public static DistributedCacheEntryOptions SlideStandard => new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(60),
        };
    }
}
