using System.Threading.Tasks;

namespace VendingMachine.Service.Products.ServiceCommunications.Client.Services
{
    public interface IProductItemClientService
    {
        Task<bool> ExistProductItemAsync(int productItemId);
    }
}
