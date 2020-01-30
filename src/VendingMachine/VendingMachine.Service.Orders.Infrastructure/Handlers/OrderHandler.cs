using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;
using VendingMachine.Service.Orders.Domain;
using VendingMachine.Service.Orders.Infrastructure.Commands;
using VendingMachine.Service.Orders.Read.Queries;
using VendingMachine.Service.Products.ServiceCommunications.Client.Services;

namespace VendingMachine.Service.Orders.Infrastructure.Handlers
{
    public class OrderHandler :
        IRequestHandler<OrderAddCommand, OrderAddResponse>,
        IRequestHandler<OrderConfirmCommand, OrderConfirmResponse>,
        IRequestHandler<OrderUpdateCommand, OrderUpdateResponse>,
        IRequestHandler<OrderAddProductItemsCommand, OrderUpdateResponse>,
        IRequestHandler<OrderRemoveProductItemsCommand, OrderUpdateResponse>,
        IRequestHandler<OrderDeleteCommand>,
        IRequestHandler<OrderClearProductItemsCommand>
    {
        private readonly IOrdersUoW ordersUoW;
        private readonly IOrderQuery orderQuery;
        private readonly IMachineClientService machineClient;
        private readonly IProductItemClientService productItemClient;

        public OrderHandler(IOrdersUoW ordersUoW, IOrderQuery orderQuery, 
            IMachineClientService machineClient, IProductItemClientService productItemClient)
        {
            this.ordersUoW = ordersUoW;
            this.orderQuery = orderQuery;
            this.machineClient = machineClient;
            this.productItemClient = productItemClient;
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
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

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

        // For massive updates
        public async Task<OrderUpdateResponse> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

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

        public async Task<OrderUpdateResponse> Handle(OrderAddProductItemsCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

            // Couple to Machine Api
            var machineStatus = await machineClient.GetMachineStatus(domain.MachineStatus.MachineId).ConfigureAwait(false);
            // Couple to Product Api
            var productItemsList = request.OrderProductItemsToAdd.Select(x => x.ProductItemId).ToList();
            var productItems = await productItemClient.GetProductItems(productItemsList);
            
            domain.UpdateMachineStatus(new MachineStatus(machineStatus.MachineId, (decimal)machineStatus.CoinCurrentSupply));
            foreach (var prod in productItems)
            {                
                var item = new OrderProductItem(prod.Id, new GrossPrice((decimal)prod.SoldPrice.GrossPrice, prod.SoldPrice.TaxPercentage));
                domain.AddProductToBasket(item);
            }
            await ordersUoW.OrderRepository.UpdateAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);
            return new OrderUpdateResponse
            {
                OrderId = domain.Id,
                CanConfirm = domain.CanConfirm
            };
        }

        public async Task<OrderUpdateResponse> Handle(OrderRemoveProductItemsCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

            // Couple to Machine Api
            var machineStatus = await machineClient.GetMachineStatus(domain.MachineStatus.MachineId).ConfigureAwait(false);

            domain.UpdateMachineStatus(new MachineStatus(machineStatus.MachineId, (decimal)machineStatus.CoinCurrentSupply));
            foreach (var prod in request.OrderProductItemsToRemove)
            {
                domain.RemoveProductToBasket(prod.ProductItemId);
            }
            await ordersUoW.OrderRepository.UpdateAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);
            return new OrderUpdateResponse
            {
                OrderId = domain.Id,
                CanConfirm = domain.CanConfirm
            };
        }

        public async Task<Unit> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

            await ordersUoW.OrderRepository.DeleteAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);
            return new Unit();
        }

        public async Task<Unit> Handle(OrderClearProductItemsCommand request, CancellationToken cancellationToken)
        {
            var domain = await ordersUoW.OrderRepository.FindAsync(request.OrderId).ConfigureAwait(false);
            if (domain == null)
                throw new System.ArgumentNullException($"No order id [{request.OrderId}] found");

            foreach (var item in domain.OrderProductItems)
            {
                domain.RemoveProductToBasket(item.Id);
            }
            await ordersUoW.OrderRepository.UpdateAsync(domain).ConfigureAwait(false);
            await ordersUoW.SaveAsync().ConfigureAwait(false);
            return new Unit();
        }
    }
}
