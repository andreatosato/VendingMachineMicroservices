using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Data;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Infrastructure
{
    public class OrdersUoW : IOrdersUoW
    {
        private readonly OrderContext db;
        public IRepository<Order> OrderRepository { get; }

        public OrdersUoW(OrderContext db, IRepository<Order> orderRepository)
        {
            this.db = db;
            OrderRepository = orderRepository;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
