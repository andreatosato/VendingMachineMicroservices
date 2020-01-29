using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Read.Models;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Orders.Read.Queries
{
    public interface IOrderQuery
    {
        Task<OrdersReadModel> GetOrders(PagedRequest pagedRequest);
        Task<OrderReadModel> GetOrder(int orderId);
    }
}
