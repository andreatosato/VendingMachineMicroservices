using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Application.Caching
{
    public static class CachingKeys
    {
        public static string ProductInformationKey(int productId) => $"ProductInformationKey-{productId}";
        public static string ProductItemInformationKey(int productId) => $"ProductItemInformationKey-{productId}";
    }
}
