using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Service.Aggregators.Web.API
{
    public class ServicesReference
    {
        public string ProductsService { get; set; }
        public string ProductItemsService { get; set; }
        public string MachineItemService { get; set; }
    }
}
