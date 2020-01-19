using System.Threading.Tasks;

namespace VendingMachine.Service.Shared.Domain
{
    public interface IRepository<T> 
        where T : IAggregateRoot
    {
        Task<T> AddAsync(T element);
        Task<T> FindAsync(int id);
        Task<T> UpdateAsync(T element);
        Task<T> DeleteAsync(T element);
    }

    public interface IRepositoryPrimaryKeys<T> : IRepository<T> 
        where T :  IAggregateRoot
    {
        int GetLatestPrimaryKey(T domain);
    }
}
