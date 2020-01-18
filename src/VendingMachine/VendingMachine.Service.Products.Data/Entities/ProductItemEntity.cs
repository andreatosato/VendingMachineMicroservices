using System;

namespace VendingMachine.Service.Products.Data.Entities
{
    public class ProductItemEntity
    {
        public int Id { get; set; }
        public ProductEntity Product { get; set; }
        public DateTimeOffset? Purchased { get; set; }
        public DateTimeOffset? Sold { get; set; }
        public DateTime ExpirationDate { get; set; }
        public GrossPriceEntity SoldPrice { get; set; }
    }
}
