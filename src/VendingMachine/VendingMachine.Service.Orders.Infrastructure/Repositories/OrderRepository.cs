using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Data;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly OrderContext db;

        public OrderRepository(OrderContext db)
        {
            this.db = db;
        }

        public async Task<Order> AddAsync(Order element)
        {
            var orderChangeTracker = await db.Orders.AddAsync(element).ConfigureAwait(false);
            return orderChangeTracker.Entity;
        }

        public async Task<Order> DeleteAsync(Order element)
        {
            var orderChangeTracker = db.Orders.Remove(element);
            return await Task.FromResult(orderChangeTracker.Entity).ConfigureAwait(false);
        }

        public async Task<Order> FindAsync(int id)
        {
            var order = await db.Orders.FindAsync(id).ConfigureAwait(false);
            return order;
        }

        public async Task<Order> UpdateAsync(Order element)
        {
            var order = db.Orders.Update(element);
            return await Task.FromResult(order.Entity).ConfigureAwait(false);
        }
    }
}
