using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Services.Shared.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> AddAsync(T element);
        Task<T> FindAsync(int id);
        Task<T> UpdateAsync(T element);
        Task<T> DeleteAsync(T element);
    }
}
