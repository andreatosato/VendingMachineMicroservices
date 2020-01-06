using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class CoinsInMachineReadModel : IReadEntity
    {
        public decimal CoinsInMachine { get; set; } 
        public decimal CoinsCurrentSupply { get; set; }
    }
}
