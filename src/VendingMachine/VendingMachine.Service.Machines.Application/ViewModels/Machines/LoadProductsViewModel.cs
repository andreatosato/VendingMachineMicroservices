using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class LoadProductsViewModel
    {
        public int MachineId { get; private set; }
        public IEnumerable<int> Products { get; set; }

        public void SetMachineId(int machineId)
        {
            this.MachineId = machineId;
        }
    }
}
