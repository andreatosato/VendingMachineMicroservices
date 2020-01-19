using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Data;
using VendingMachine.Service.Products.Data.Entities;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Infrastructure.Repositories
{
    public class ProductRepository : IRepositoryPrimaryKeys<Product>
    {
        private readonly ProductContext db;
        private readonly IMapper mapper;
        private readonly IDictionary<Product, ProductEntity> addedObjects = new Dictionary<Product, ProductEntity>();

        public ProductRepository(ProductContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Product> AddAsync(Product element)
        {
            if (element is Snack)
            {
                var entity = await db.Products.AddAsync(mapper.Map<SnackEntity>(element)).ConfigureAwait(false);
                addedObjects.Add(element, entity.Entity);
                return mapper.Map<Snack>(entity.Entity);
            }
            if (element is ColdDrink)
            {
                var entity = await db.Products.AddAsync(mapper.Map<ColdDrinkEntity>(element)).ConfigureAwait(false);
                addedObjects.Add(element, entity.Entity);
                return mapper.Map<ColdDrink>(entity.Entity);
            }
            if (element is HotDrink)
            {
                var entity = await db.Products.AddAsync(mapper.Map<HotDrinkEntity>(element)).ConfigureAwait(false);
                addedObjects.Add(element, entity.Entity);
                return mapper.Map<HotDrink>(entity.Entity);
            }
            throw new ArgumentException("Can't add product");
        }

        public async Task<Product> DeleteAsync(Product element)
        {
            if (element is Snack)
                db.Products.Remove(mapper.Map<SnackEntity>(element));
            else if(element is ColdDrink)
                db.Products.Remove(mapper.Map<ColdDrinkEntity>(element));
            else if(element is HotDrink)
                db.Products.Remove(mapper.Map<HotDrinkEntity>(element));
            return await Task.FromResult(element).ConfigureAwait(false);
        }

        public async Task<Product> FindAsync(int id)
        {
            var entity = await db.Products.FindAsync(id);
            if (entity is ColdDrinkEntity)
                return mapper.Map<ColdDrink>(entity);
            if (entity is HotDrinkEntity)
                return mapper.Map<HotDrink>(entity);
            if (entity is SnackEntity)
                return mapper.Map<Snack>(entity);
            return await Task.FromResult<Product>(null);
        }

        public async Task<Product> UpdateAsync(Product element)
        {
            if (element is ColdDrink)
                db.Products.Update(mapper.Map<ColdDrinkEntity>(element));
            if (element is HotDrink)
                db.Products.Update(mapper.Map<HotDrinkEntity>(element));
            if (element is Snack)
                db.Products.Update(mapper.Map<SnackEntity>(element));
            return await Task.FromResult(element).ConfigureAwait(false);
        }

        public int GetLatestPrimaryKey(Product domain)
        {
            if (addedObjects.TryGetValue(domain, out ProductEntity entity))
            {
                addedObjects.Remove(domain);
                return entity.Id;
            }
            throw new InvalidOperationException("No entity found");
        }
    }
}
