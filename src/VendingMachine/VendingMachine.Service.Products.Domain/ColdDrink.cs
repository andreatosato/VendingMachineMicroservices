using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Domain
{
    /// Bottle Drink 
    public class ColdDrink : Product
    {
        public decimal TemperatureMinimum { get; private set; }
        public decimal TemperatureMaximum { get; private set; }
        public decimal Litre { get; }

        public ColdDrink(string name, decimal litre)
            : base(name)
        {
            Litre = litre;
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
