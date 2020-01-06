using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure.Repositories
{
    public class MachineItemRepository : IRepository<MachineItem>
    {
        private readonly MachineContext db;

        public MachineItemRepository(MachineContext db)
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
            db.RemoveRange(resultEntity.ActiveProducts);
            db.RemoveRange(resultEntity.HistoryProducts);
            return await Task.FromResult(resultEntity);
        }

        public async Task<MachineItem> FindAsync(int id)
        {
            return await db.Machines
                .Include(x => x.ActiveProducts)
                .Include(x => x.HistoryProducts)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<MachineItem> UpdateAsync(MachineItem element)
        {
            var resultEntity = db.Machines.Update(element).Entity;
            return await Task.FromResult(resultEntity);
        }
    }
}
