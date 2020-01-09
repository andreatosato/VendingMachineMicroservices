using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public interface IMachinesUoW : IUnitOfWork
    {
        IRepository<MachineItem> MachineRepository { get; }
        IRepository<MachineType> MachineTypeRepository { get; }
    }
}
