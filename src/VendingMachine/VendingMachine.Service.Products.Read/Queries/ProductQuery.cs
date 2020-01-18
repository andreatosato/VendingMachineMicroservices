using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Read.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly string productConnectionString;

        public ProductQuery(string productConnectionString)
        {
            this.productConnectionString = productConnectionString;
        }
    }
}
