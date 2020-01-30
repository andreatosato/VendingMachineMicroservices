using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Orders.Infrastructure.Commands;
using VendingMachine.Service.Orders.Read.Queries;

namespace VendingMachine.Service.Orders.Infrastructure.Handlers
{
    public class OrderHandler :
        IRequestHandler<OrderAddCommand, OrderAddResponse>,
        IRequestHandler<OrderConfirmCommand, OrderConfirmResponse>,
        IRequestHandler<OrderUpdateCommand, OrderUpdateResponse>
    {
        private readonly IOrdersUoW ordersUoW;
        private readonly IOrderQuery orderQuery;

        public OrderHandler(IOrdersUoW ordersUoW, IOrderQuery orderQuery)
        {
            this.ordersUoW = ordersUoW;
            this.orderQuery = orderQuery;
        }

        public async Task<OrderAddResponse> Handle(OrderAddCommand request, CancellationToken cancellationToken)
        {
            // Check pending order for this machine
            bool orderInPending = await orderQuery.ExistPendingOrder(request.MachineStatus.MachineId);
            if (orderInPending)
                throw new System.InvalidOperationException("Another order in pending for this machine");

            MachineStatus machineStatus = new MachineStatus(request.MachineStatus.MachineId, request.MachineStatus.CoinsCurrentSupply);
            List<OrderProductItem> productItems = new List<OrderProductItem>();
            foreach (var p in request.OrderProducts)
            {
                var price = new GrossPrice(p.Price.GrossPrice, p.Price.TaxPercentage);
                productItems.Add(new OrderProductItem(p.ProductItemId, price));
            }
            Order domain = new Order(machineStatus, productItems, request.OrderDate);
            var orderInserted = await ordersUoW.OrderRepository.AddAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);

            return new OrderAddResponse()
            {
                OrderId = orderInserted.Id,
                CanConfirm = orderInserted.CanConfirm
            };
        }

        public async Task<OrderConfirmResponse> Handle(OrderConfirmCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId);
            if (domain.CanConfirm)
            {
                domain.ConfirmOrder();
                var orderUpdated = await ordersUoW.OrderRepository.UpdateAsync(domain).ConfigureAwait(false);
                await ordersUoW.SaveAsync().ConfigureAwait(false);
                return new OrderConfirmResponse { Confirmed = orderUpdated.Processed };
            }
            else
                return new OrderConfirmResponse { Confirmed = domain.Processed };
        }

        public async Task<OrderUpdateResponse> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            foreach (var prod in request.OrderProductsToAdd)
            {
                var item = new OrderProductItem(prod.ProductItemId, 
                                new GrossPrice(prod.Price.GrossPrice, prod.Price.TaxPercentage));
                domain.AddProductToBasket(item);
            }

            foreach (var prod in request.OrderProductsToRemove)
            {
                domain.RemoveProductToBasket(prod.ProductItemId);
            }

            domain.UpdateMachineStatus(new MachineStatus(request.MachineStatus.MachineId, request.MachineStatus.CoinsCurrentSupply));
            await ordersUoW.OrderRepository.UpdateAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);
            return new OrderUpdateResponse
            {
                OrderId = domain.Id,
                CanConfirm = domain.CanConfirm
            };
        }
    }
}
