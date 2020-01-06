using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class AddCoinsMachineCommand : IRequest
    {
        public int MachineId { get; set; }
        public decimal CoinsAdded { get; set; }
    }
}
