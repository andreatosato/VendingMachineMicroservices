using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Infrastructure
{
    public interface IOrdersUoW : IUnitOfWork
    {
        IRepository<Order> OrderRepository { get; }
    }
}
