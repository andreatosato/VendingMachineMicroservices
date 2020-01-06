using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Machines.Infrastructure.Repositories;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public class MachinesUoW : IMachinesUoW
    {
        private readonly MachineContext db;

        public IRepository<MachineItem> MachineRepository { get; }
        public IRepository<MachineType> MachineTypeRepository { get; }

        public MachinesUoW(MachineContext db, 
            IRepository<MachineItem> machineRepository, 
            IRepository<MachineType> machineTypeRepository)
        {
            this.db = db;
            MachineRepository = machineRepository;
            MachineTypeRepository = machineTypeRepository;
        }
        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
