using MediatR;
using System;

namespace VendingMachine.Service.Products.Infrastructure.Commands
{
    public class ProductItemAddCommand : IRequest<ProductItemAddResponse>
    {
        public int ProductId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class ProductItemAddResponse
    {
        public int ProductItemId { get; set; }
    }

    public class ProductItemPurchaseCommand : IRequest
    {
        public int ProductItemId { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
    }

    public class ProductItemSoldCommand : IRequest
    {
        public int ProductItemId { get; set; }
        public DateTimeOffset SoldDate { get; set; }
    }

    public class ProductItemRedefinePriceCommand : IRequest
    {
        public int ProductItemId { get; set; }
        public decimal NewPrice { get; set; }
    }
}
