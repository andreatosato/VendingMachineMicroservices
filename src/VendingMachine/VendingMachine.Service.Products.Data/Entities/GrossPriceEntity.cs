using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Data.Entities
{
    [Owned]
    public class GrossPriceEntity
    {
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public int TaxPercentage { get; set; }
        public decimal Rate { get; set; }
    }
}
