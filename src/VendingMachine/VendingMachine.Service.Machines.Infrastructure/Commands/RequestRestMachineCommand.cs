using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class RequestRestMachineCommand : IRequest<decimal>
    {
        public int MachineId { get; set; }
        public decimal Rest { get; set; }
    }
}
