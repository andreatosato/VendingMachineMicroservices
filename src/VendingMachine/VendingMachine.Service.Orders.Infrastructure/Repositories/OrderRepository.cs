using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        public OrderRepository()
        {
        }

        public Task<Order> AddAsync(Order element)
        {
            throw new NotImplementedException();
        }

        public Task<Order> DeleteAsync(Order element)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order element)
        {
            throw new NotImplementedException();
        }
    }
}
