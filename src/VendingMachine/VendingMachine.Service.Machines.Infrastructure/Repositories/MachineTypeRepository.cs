using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines.Infrastructure.Repositories
{
    public class MachineTypeRepository : IRepository<MachineType>
    {
        public Task<MachineType> AddAsync(MachineType element)
        {
            throw new NotImplementedException();
        }

        public Task<MachineType> DeleteAsync(MachineType element)
        {
            throw new NotImplementedException();
        }

        public Task<MachineType> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MachineType> UpdateAsync(MachineType element)
        {
            throw new NotImplementedException();
        }
    }
}
