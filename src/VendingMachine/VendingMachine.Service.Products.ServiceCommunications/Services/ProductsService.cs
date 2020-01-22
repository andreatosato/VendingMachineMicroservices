using Grpc.Core;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products.ServiceCommunications.Services
{
    public class ProductsService : Products.ProductsBase
    {
        private readonly IProductQuery query;

        public ProductsService(IProductQuery query)
        {
            this.query = query;
        }

        public override async Task<ExistProductResponse> ExistProduct(ExistProductRequest request, ServerCallContext context)
        {
            bool result = await query.ExistProductAsync(request.ProductId);
            return new ExistProductResponse() { Exist = result };
        }
    }
}
