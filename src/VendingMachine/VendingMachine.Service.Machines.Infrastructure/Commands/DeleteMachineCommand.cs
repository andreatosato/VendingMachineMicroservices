using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class DeleteMachineCommand : IRequest
    {
        public int MachineId { get; set; }
    }
}
