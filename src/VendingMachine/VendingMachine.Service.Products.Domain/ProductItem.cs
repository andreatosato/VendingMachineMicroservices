using System;
using VendingMachine.Service.Products.Domain.DomainEvents;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Domain
{
    public class ProductItem : Entity, IAggregateRoot
    {
        public DateTimeOffset? Purchased { get; private set; }
        public DateTimeOffset? Sold { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public Product Product { get; }
        public GrossPrice SoldPrice { get; }

        public ProductItem(Product product)
        {
            Product = product;
            // Set price equal standard product
            SoldPrice = product.Price;
        }

        public void SetExpirationDate(DateTime date)
        {
            ExpirationDate = date;
            AddDomainEvent(new ProductItemExpirationDateEvent() { Expiration = ExpirationDate, ProductItemId = Id });
        }

        public void SetPurchasedDate(DateTimeOffset date)
        {
            Purchased = date;
        }

        public void SetSoldDate(DateTimeOffset date)
        {
            Sold = date;
        }
        /// <summary>
        /// Assume that tax can't change
        /// </summary>
        /// <param name="price">price to set</param>
        public void SetGrossPriceValue(decimal newPrice)
        {
            SoldPrice.RedefinePrice(newPrice);
        }
    }
}
