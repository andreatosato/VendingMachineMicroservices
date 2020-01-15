using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Domain
{
    public class Snak : Product
    {
        public decimal Grams { get; }

        public Snak(string name, decimal grams) : base(name)
        {
            Grams = grams;
        }
    }
}
