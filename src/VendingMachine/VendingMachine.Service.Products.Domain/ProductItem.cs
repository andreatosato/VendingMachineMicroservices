using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Domain
{
    public class ProductItem : Entity, IAggregateRoot
    {
        public Product Product { get; }

        public ProductItem(Product product)
        {
            Product = product;
        }
    }
}
