using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Data;
using VendingMachine.Service.Products.Data.Entities;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Infrastructure.Repositories
{
    public class ProductItemRepository : IRepository<ProductItem>
    {
        private readonly ProductContext db;
        private readonly IMapper mapper;

        public ProductItemRepository(ProductContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ProductItem> AddAsync(ProductItem element)
        {
            var entity = await db.AddAsync(element).ConfigureAwait(false);
            return mapper.Map<ProductItem>(entity.Entity);           
        }

        public async Task<ProductItem> DeleteAsync(ProductItem element)
        {
            db.Remove(mapper.Map<ProductItemEntity>(element));
            return await Task.FromResult(element).ConfigureAwait(false);
        }

        public async Task<ProductItem> FindAsync(int id)
        {
            var entity = await db.ProductItems.FindAsync(id).ConfigureAwait(false);
            return mapper.Map<ProductItem>(entity);
        }

        public async Task<ProductItem> UpdateAsync(ProductItem element)
        {
            db.ProductItems.Update(mapper.Map<ProductItemEntity>(element));
            return await Task.FromResult(element).ConfigureAwait(false);
        }
    }
}
