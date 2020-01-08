using System.Collections.Generic;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class BuyProductsViewModel
    {
        public IEnumerable<int> Products { get; set; }
        public decimal TotalBuy { get; set; }
        public decimal TotalRest { get; set; }
    }
}
