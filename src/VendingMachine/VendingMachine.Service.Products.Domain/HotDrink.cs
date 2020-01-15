using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Domain
{
    public class HotDrink : Product
    {
        public decimal TemperatureMinimum { get; private set; }
        public decimal TemperatureMaximum { get; private set; }
        public decimal Grams { get; }

        public HotDrink(string name, decimal grams) 
            : base(name)
        {
            Grams = grams;
        }

        public void SetTemperatureMinimum(decimal temp)
        {
            TemperatureMinimum = temp;
        }

        public void SetTemperatureMaximun(decimal temp)
        {
            TemperatureMaximum = temp;
        }
    }
}
