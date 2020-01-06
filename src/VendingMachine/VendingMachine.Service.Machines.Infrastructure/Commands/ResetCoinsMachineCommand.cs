using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class ResetCoinsMachineCommand : IRequest<ResetCoinsMachineResult>
    {
        public int MachineId { get; set; }
    }

    public class ResetCoinsMachineResult
    {
        public decimal RestCoins { get; set; }
    }
}
