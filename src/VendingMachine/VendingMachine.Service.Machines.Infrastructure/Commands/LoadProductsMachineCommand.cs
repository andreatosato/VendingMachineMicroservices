using MediatR;
using System.Collections.Generic;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class LoadProductsMachineCommand : IRequest
    {
        public int MachineId { get; set; }
        public IEnumerable<int> Products { get; set; }
    }
}
