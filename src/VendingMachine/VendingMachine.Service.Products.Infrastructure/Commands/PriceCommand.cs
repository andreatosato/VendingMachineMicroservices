using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Infrastructure.Commands
{
    public class PriceCommand
    {
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
