using System;
using System.Collections.Generic;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Domain
{
    public class OrderProductItem : Entity, IAggregateRoot
    {
        public int ProductItemId { get; }
        public GrossPrice Price { get; }

        public OrderProductItem(int productItemId, GrossPrice price)
        {
            if (productItemId == 0)
                throw new ArgumentException("ProductItem must be greater then 0");

            
            ProductItemId = productItemId;
            Price = price;
        }
    }

    public class GrossPrice : ValueObject
    {
        public decimal Value { get; private set; }
        public decimal NetPrice { get; private set; }
        public int TaxPercentage { get; }
        public decimal Rate { get; private set; }

        public GrossPrice(decimal price, int taxPercentage)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException("Price must be greater or equal then 0");
            if (taxPercentage < 0)
                throw new ArgumentOutOfRangeException("Tax Percentage must be greater or equal then 0");
            if (taxPercentage < 0 || taxPercentage > 100)
                throw new ArgumentOutOfRangeException("Tax Percentage must be less or equal then 100");

            Value = price;
            TaxPercentage = taxPercentage;
            CalculatePriceAndRate();
        }

        public void RedefinePrice(decimal newPrice)
        {
            Value = newPrice;
            CalculatePriceAndRate();
        }

        private void CalculatePriceAndRate()
        {
            Rate = Value * ((decimal)TaxPercentage / 100);
            NetPrice = Value * ((100 - (decimal)TaxPercentage) / 100);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return TaxPercentage;
        }
    }
}
