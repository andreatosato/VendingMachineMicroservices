using System;
using VendingMachine.Service.Products.Application.ViewModels.Products;

namespace VendingMachine.Service.Products.Application.ViewModels.ProductItems
{
    //public class ProductItemViewModel<T>
    //    where T : ProductViewModel
    //{
    //    public T Product { get; set; }
    //    public decimal? SoldPrice { get; set; }
    //    public DateTime? ExpirationDate { get; set; }
    //    public DateTimeOffset? Purchased { get; set; }
    //    public DateTimeOffset? Sold { get; set; }
    //}

    public class ProductItemViewModel
    {
        public int ProductId { get; set; }
        public decimal? SoldPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTimeOffset? Purchased { get; set; }
        public DateTimeOffset? Sold { get; set; }
    }
}
