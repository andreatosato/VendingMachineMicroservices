using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class CollectCoinsMachineCommand : IRequest<CollectCoinsMachineResult>
    {
        public int MachineId { get; set; }
    }

    public class CollectCoinsMachineResult
    {
        public decimal CoinsCollected { get; set; }
    }
}
