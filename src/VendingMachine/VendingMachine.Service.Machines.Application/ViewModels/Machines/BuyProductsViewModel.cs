using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class BuyProductsViewModel
    {
        public List<int> Products { get; set; }
        public decimal TotalBuy { get; set; }
        public decimal TotalRest { get; set; }
    }
}
