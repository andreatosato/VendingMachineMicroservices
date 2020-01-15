using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Data.Entities
{
    public class HotDrinkEntity : ProductEntity
    {
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Grams { get; set; }
    }
}
