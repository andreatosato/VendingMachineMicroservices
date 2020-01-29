using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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

        public async Task<ICollection<ProductItemsServiceModel>> GetProductItems(List<int> productItems)
        {
            Collection<ProductItemsServiceModel> products = new Collection<ProductItemsServiceModel>();
            var request = new GetProductItemsRequest();
            request.ProductIds.AddRange(productItems);
            using (var productStream = productItemsClient.GetProductItems(request))
            {
                while (await productStream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    ProductItemsServiceModel product = productStream.ResponseStream.Current;
                    products.Add(product);
                }
            }
            return products;
        }
    }
}
