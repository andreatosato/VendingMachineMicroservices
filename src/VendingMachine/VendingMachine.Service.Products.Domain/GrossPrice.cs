using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Domain
{
    public class GrossPrice : ValueObject
    {
        public decimal Value { get; }
        public decimal NetPrice { get; }
        public int TaxPercentage { get; }
        public decimal Rate { get; }

        public GrossPrice(decimal price, int taxPercentage)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException("Price must be greater or equal then 0");
            if(taxPercentage < 0)
                throw new ArgumentOutOfRangeException("Tax Percentage must be greater or equal then 0");

            Value = price;
            TaxPercentage = taxPercentage;
            Rate = Value * TaxPercentage;
            NetPrice = Value * (100 - TaxPercentage);
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return TaxPercentage;
        }
    }
}
