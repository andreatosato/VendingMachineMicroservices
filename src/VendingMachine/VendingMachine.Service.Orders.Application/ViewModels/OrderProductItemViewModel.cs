using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Application.ViewModels
{
    public class OrderProductItemViewModel
    {
        public int ProductItem { get; set; }
        public GrossPriceViewModel Price { get; set; }
    }

    public class GrossPriceViewModel
    {
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
