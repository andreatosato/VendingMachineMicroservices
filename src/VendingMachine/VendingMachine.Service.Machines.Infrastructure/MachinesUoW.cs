using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Infrastructure.Repositories;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public class MachinesUoW : IMachinesUoW
    {
        private readonly MachineContext db;

        public MachineRepository MachineRepository { get; }
        public MachineTypeRepository MachineTypeRepository { get; }

        public MachinesUoW(MachineContext db, MachineRepository machineRepository, MachineTypeRepository machineTypeRepository)
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
