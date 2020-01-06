using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Machines.Infrastructure.Repositories;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public interface IMachinesUoW : IUnitOfWork
    {
        IRepository<MachineItem> MachineRepository { get; }
        IRepository<MachineType> MachineTypeRepository { get; }
    }
}
