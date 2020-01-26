using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    public class OrderUpdateCommand : IRequest<OrderUpdateResponse>
    {
        public IEnumerable<OrderProductItemCommand> OrderProducts { get; set; }
        public MachineStatusCommand MachineStatus { get; set; }
    }

    public class OrderUpdateResponse
    {
        public int OrderId { get; set; }
        public bool CanConfirm { get; set; }
    }
}
