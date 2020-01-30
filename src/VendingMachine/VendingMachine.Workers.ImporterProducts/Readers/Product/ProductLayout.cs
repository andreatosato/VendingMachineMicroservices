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
                .WithMember(x => x.GrossPrice)
                .WithMember(x => x.TaxPercentage);
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
