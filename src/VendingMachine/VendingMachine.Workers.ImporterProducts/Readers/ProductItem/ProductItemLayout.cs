using FlatFile.Delimited.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public sealed class ProductItemLayout : DelimitedLayout<ProductItem>
    {
        public ProductItemLayout()
        {
            WithDelimiter(";")
                .WithQuote("\"")
                .WithMember(x => x.MachineId)
                .WithMember(x => x.ProductId)
                .WithMember(x => x.Purchased)
                .WithMember(x => x.ExpirationDate)
                .WithMember(x => x.GrossPrice, c => c.AllowNull(""))
                .WithMember(x => x.TaxPercentage, c => c.AllowNull(""));
        }
    }

    public class ProductItem
    {
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public DateTimeOffset Purchased { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
