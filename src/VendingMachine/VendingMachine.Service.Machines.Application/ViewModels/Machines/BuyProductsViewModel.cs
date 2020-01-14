using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class BuyProductsViewModel
    {
        public int MachineId { get; private set; }
        public IEnumerable<int> Products { get; set; }
        public decimal TotalBuy { get; set; }
        public decimal TotalRest { get; set; }
        public void SetMachineId(int value)
        {
            MachineId = value;
        }
    }

    
}
