using System;
using VendingMachine.Service.Products.Read.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VendingMachine.Service.Products.Read.Queries
{
    public interface IProductQuery
    {
        Task<ProductReadModel> GetProductInfoAsync(int productId);
        Task<ProductsReadModel> GetProductsInfoAsync(List<int> productId);
        Task<bool> ExistProductAsync(int productId);
    }
}
