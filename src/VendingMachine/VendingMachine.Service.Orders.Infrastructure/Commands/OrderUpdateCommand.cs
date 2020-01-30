using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    // For Massive Update
    public class OrderUpdateCommand : IRequest<OrderUpdateResponse>
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderProductItemCommand> OrderProductsToAdd { get; set; }
        public IEnumerable<OrderProductItemBaseCommand> OrderProductsToRemove { get; set; }
        public MachineStatusCommand MachineStatus { get; set; }
    }

    public class OrderAddProductItemsCommand : IRequest<OrderUpdateResponse>
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderProductItemBaseCommand> OrderProductItemsToAdd { get; set; }
    }

    public class OrderRemoveProductItemsCommand : IRequest<OrderUpdateResponse>
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderProductItemBaseCommand> OrderProductItemsToRemove { get; set; }
    }

    public class OrderClearProductItemsCommand : IRequest
    {
        public int OrderId { get; set; }
    }

    public class OrderUpdateResponse
    {
        public int OrderId { get; set; }
        public bool CanConfirm { get; set; }
    }
}
