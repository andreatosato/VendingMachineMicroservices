using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Read.Queries
{
    public class ProductItemQuery : IProductItemQuery
    {
        private readonly string productConnectionString;

        public ProductItemQuery(string productConnectionString)
        {
            this.productConnectionString = productConnectionString;
        }
    }
}
