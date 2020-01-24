using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VendingMachine.Service.Products.Read.Models
{
    public class ProductItemsReadModel
    {
        public IEnumerable<ProductItemReadModel> Products { get; set; } = new Collection<ProductItemReadModel>();
    }
    public class ProductItemReadModel
    {
        public int Id { get; set; }
        public ProductReadModel Product { get; set; }
        public DateTimeOffset? Purchased { get; set; }
        public DateTimeOffset? Sold { get; set; }
        public DateTime ExpirationDate { get; set; }
        public GrossPriceReadModel SoldPrice { get; }
    }
}
