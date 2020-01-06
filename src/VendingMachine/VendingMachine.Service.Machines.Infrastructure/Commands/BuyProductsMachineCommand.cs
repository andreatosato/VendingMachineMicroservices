using MediatR;
using System.Collections.Generic;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class BuyProductsMachineCommand : IRequest
    {
        public int MachineId { get; set; }
        public IEnumerable<int> ProductsBuy { get; set; }
        public decimal TotalBuy { get; set; }
        public decimal TotalRest { get; set; }
    }
}
