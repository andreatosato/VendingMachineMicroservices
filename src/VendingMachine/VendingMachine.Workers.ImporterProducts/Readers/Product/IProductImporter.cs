using System.Threading.Tasks;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public interface IProductImporter
    {
        Task DoWorkAsync(string Name, string FullName);
    }
}
