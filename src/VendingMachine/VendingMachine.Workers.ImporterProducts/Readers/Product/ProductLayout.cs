using FlatFile.Delimited.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public sealed class ProductLayout : DelimitedLayout<Product>
    {
        public ProductLayout()
        {
            WithDelimiter(";")
                .WithQuote("\"")
                .WithMember(x => x.Name)
                .WithMember(x => x.ProductType)
                .WithMember(x => x.GrossPrice)
                .WithMember(x => x.TaxPercentage)
                .WithMember(x => x.Litre, c => c.AllowNull(""))
                .WithMember(x => x.TemperatureMaximum, c => c.AllowNull(""))
                .WithMember(x => x.TemperatureMinimum, c => c.AllowNull(""))
                .WithMember(x => x.Grams, c => c.AllowNull(""));
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
        public string ProductType { get; set; }
        public decimal Litre { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal TemperatureMinimum { get; set; }
        public decimal Grams { get; set; }
    }
}
