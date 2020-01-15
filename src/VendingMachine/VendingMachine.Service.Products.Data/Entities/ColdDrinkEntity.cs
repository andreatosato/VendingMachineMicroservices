using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Data.Entities
{
    public class ColdDrinkEntity : ProductEntity
    {
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Litre { get; set; }
    }
}
