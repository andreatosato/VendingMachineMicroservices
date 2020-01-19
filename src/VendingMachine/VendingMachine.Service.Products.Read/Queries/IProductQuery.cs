using System;
using VendingMachine.Service.Products.Read.Models;
using System.Threading.Tasks;

namespace VendingMachine.Service.Products.Read.Queries
{
    public interface IProductQuery
    {
        Task<ProductReadModel> GetProductInfoAsync(int productId);
    }
}
