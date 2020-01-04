using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure.Repositories
{
    public class MachineRepository : IRepository<MachineItem>
    {
        private readonly MachineContext db;

        public MachineRepository(MachineContext db)
        {
            this.db = db;
        }

        public async Task<MachineItem> AddAsync(MachineItem element)
        {
            var entityResult = (await db.AddAsync(element).ConfigureAwait(false)).Entity;
            return entityResult;
        }

        public async Task<MachineItem> DeleteAsync(MachineItem element)
        {
            var resultEntity = db.Remove(element).Entity;
            return await Task.FromResult(resultEntity);
        }

        public async Task<MachineItem> FindAsync(int id)
        {
            return await db.Machines.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<MachineItem> UpdateAsync(MachineItem element)
        {
            var resultEntity = db.Machines.Update(element).Entity;
            return await Task.FromResult(resultEntity);
        }
    }
}
