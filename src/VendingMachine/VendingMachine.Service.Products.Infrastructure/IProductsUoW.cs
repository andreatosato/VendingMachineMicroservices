using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Infrastructure
{
    public interface IProductsUoW : IUnitOfWork
    {
        IRepositoryPrimaryKeys<Product> ProductRepository { get; }
        IRepositoryPrimaryKeys<ProductItem> ProductItemRepository { get; }
    }
}
