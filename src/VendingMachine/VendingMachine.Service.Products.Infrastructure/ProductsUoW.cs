using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Data;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Infrastructure
{
    public class ProductsUoW : IProductsUoW
    {
        private readonly ProductContext db;
        private readonly ILogger logger;
        public IRepositoryPrimaryKeys<Product> ProductRepository { get; }
        public IRepositoryPrimaryKeys<ProductItem> ProductItemRepository { get; }

        public ProductsUoW(ProductContext db, 
            ILoggerFactory loggerFactory,
            IRepositoryPrimaryKeys<Product> productRepository, 
            IRepositoryPrimaryKeys<ProductItem> productItemRepository)
        {
            this.db = db;
            logger = loggerFactory.CreateLogger(typeof(ProductsUoW));
            ProductRepository = productRepository;
            ProductItemRepository = productItemRepository;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duUpdateEx)
            {
                logger.LogError(duUpdateEx, "Error to save data in context id: {contextId}", db.ContextId);
                throw duUpdateEx;
            }
        }
    }
}
