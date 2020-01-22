using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Service.Aggregators.Web.API
{
    public interface IServicesReference
    {
        string ProductsService { get; set; }
        string ProductItemsService { get; set; }
        string MachineItemService { get; set; }
    }

    public class ServicesReference : IServicesReference
    {
        public string ProductsService { get; set; }
        public string ProductItemsService { get; set; }
        public string MachineItemService { get; set; }
    }
}
