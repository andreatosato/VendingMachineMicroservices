using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure
{
    public class MachinesUoW : IMachinesUoW
    {
        private readonly MachineContext db;
        private readonly ILogger logger;

        public IRepository<MachineItem> MachineRepository { get; }
        public IRepository<MachineType> MachineTypeRepository { get; }

        public MachinesUoW(MachineContext db, 
            IRepository<MachineItem> machineRepository, 
            IRepository<MachineType> machineTypeRepository,
            ILoggerFactory loggerFactory)
        {
            this.db = db;
            this.logger = loggerFactory.CreateLogger(typeof(IMachinesUoW));
            MachineRepository = machineRepository;
            MachineTypeRepository = machineTypeRepository;
        }

        public void Save()
        {
            this.SaveAsync().Wait();
        }

        public async Task SaveAsync()
        {
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duUpdateEx)
            {
                logger.LogError(duUpdateEx, "Error to save data in context id: {contextId}", db.ContextId);
                throw duUpdateEx;
            }
        }
    }
}
