using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Infrastructure
{
    public interface IProductsUoW : IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<ProductItem> ProductItemRepository { get; }
    }
}
