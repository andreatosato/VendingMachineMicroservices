using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure.Repositories
{
    public class MachineRepository : IRepository<Machine>
    {
        private readonly MachineContext db;

        public MachineRepository(MachineContext db)
        {
            this.db = db;
        }

        public async Task<Machine> AddAsync(Machine element)
        {
            var entityResult = (await db.AddAsync(element).ConfigureAwait(false)).Entity;
            return entityResult;
        }

        public async Task<Machine> DeleteAsync(Machine element)
        {
            var resultEntity = db.Remove(element).Entity;
            return await Task.FromResult(resultEntity);
        }

        public async Task<Machine> FindAsync(int id)
        {
            return await db.Machines.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<Machine> UpdateAsync(Machine element)
        {
            var resultEntity = db.Machines.Update(element).Entity;
            return await Task.FromResult(resultEntity);
        }
    }
}
