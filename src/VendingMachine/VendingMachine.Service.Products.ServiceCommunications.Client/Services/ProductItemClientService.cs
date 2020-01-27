using System.Threading.Tasks;

namespace VendingMachine.Service.Products.ServiceCommunications.Client.Services
{
    public class ProductItemClientService : IProductItemClientService
    {
        private readonly ProductItems.ProductItemsClient productItemsClient;

        public ProductItemClientService(ProductItems.ProductItemsClient productItemsClient)
        {
            this.productItemsClient = productItemsClient;
        }

        public async Task<bool> ExistProductItemAsync(int productItemId)
        {
            var request = new ExistProductItemRequest { ProductItemId = productItemId };
            var response = await productItemsClient.ExistProductItemAsync(request);
            return response.Exist;
        }
    }
}
