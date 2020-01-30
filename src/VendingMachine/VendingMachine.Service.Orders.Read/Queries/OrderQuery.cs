using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Read.Models;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Orders.Read.Queries
{
    public class OrderQuery : IOrderQuery
    {
        private readonly string orderConnectionString;
        public OrderQuery(string orderConnectionString)
        {
            this.orderConnectionString = orderConnectionString;
        }

        public async Task<OrdersReadModel> GetOrders(PagedRequest pagedRequest)
        {
            OrdersReadModel result = new OrdersReadModel();
            
            using SqlConnection connection = new SqlConnection(orderConnectionString);
            await connection
                .QueryAsync<OrderReadModel, OrderProductItemReadModel, OrderReadModel>(
                    @"SELECT O.[Id],[MachineId],[CoinsCurrentSupply],[OrderDate],[Processed],
                    OI.[Id],[ProductItemId],[GrossPrice],[NetPrice],[TaxPercentage],[Rate]
                    FROM [dbo].[Orders] O
                    INNER JOIN [dbo].[OrderProductItem] OI ON O.Id = OI.OrderId
                    ORDER BY O.Id DESC
                    OFFSET     @Skip ROWS       -- skip 10 rows
                    FETCH NEXT @TAKE ROWS ONLY",
                map: (order, orderItems) =>
                {
                    OrderReadModel currentOrder;
                    if(result.Entities.Any(x => x.Id == order.Id))
                    {
                        currentOrder = result.Entities.FirstOrDefault(x => x.Id == order.Id);
                    }
                    else
                    {
                        currentOrder = order;
                        result.Entities.Add(currentOrder);
                    }
                    currentOrder.OrderProductItems.Add(orderItems);
                    return currentOrder;
                },
                param: new { pagedRequest.Skip, pagedRequest.Take }).ConfigureAwait(false);
            
            result.CurrentPage = pagedRequest.Skip / pagedRequest.Take;

            int total = await connection
               .ExecuteScalarAsync<int>(
                   @"SELECT COUNT(O.Id)
                    FROM [dbo].[Orders] O
                    INNER JOIN [dbo].[OrderProductItem] OI ON O.Id = OI.OrderId");
            
            result.Total = total;
            return result;
        }

        public async Task<OrderReadModel> GetOrder(int orderId)
        {
            OrderReadModel result = new OrderReadModel();

            using SqlConnection connection = new SqlConnection(orderConnectionString);
            await connection
                .QueryAsync<OrderReadModel, OrderProductItemReadModel, OrderReadModel>(
                    @"SELECT O.[Id],[MachineId],[CoinsCurrentSupply],[OrderDate],[Processed],
                    OI.[Id],[ProductItemId],[GrossPrice],[NetPrice],[TaxPercentage],[Rate]
                    FROM [dbo].[Orders] O
                    INNER JOIN [dbo].[OrderProductItem] OI ON O.Id = OI.OrderId
                    WHERE O.Id = @Id",
                map: (order, orderItem) =>
                {
                    if (!result.OrderProductItems.Any(x => x.ProductItemId == orderItem.ProductItemId))
                    {
                        result.OrderProductItems.Add(orderItem);
                    }
                    return result;
                },
                param: new { Id = orderId }).ConfigureAwait(false);

            return result;
        }

        public async Task<bool> ExistPendingOrder(int machineId)
        {
            using SqlConnection connection = new SqlConnection(orderConnectionString);
            bool result = await connection
                .ExecuteScalarAsync<bool>(
                    @"SELECT COUNT(DISTINCT 1)
                    FROM [dbo].[Orders]
                    WHERE MachineId = @Id 
                    AND (Confirmed = 0 AND Cancelled = 0)  /*Not Processed*/",
                param: new { Id = machineId })
                .ConfigureAwait(false);

            return result;
        }
    }
}
