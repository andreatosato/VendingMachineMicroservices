using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Infrastructure.Repositories;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public interface IMachinesUoW : IUnitOfWork
    {
        MachineRepository MachineRepository { get; }
        MachineTypeRepository MachineTypeRepository { get; }
    }
}
