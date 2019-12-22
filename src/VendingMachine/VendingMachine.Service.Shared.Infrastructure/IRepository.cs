using System;
using System.Threading.Tasks;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Shared.Infrastructure
{
    public interface IRepository<T> where T: IAggregateRoot
    {
        T AddAsync(T element);
        T UpdateAsync(T element);
        Task<T> FindByIdAsync(int id);
        Task DeleteByIdAsync(int id);
    }
}
