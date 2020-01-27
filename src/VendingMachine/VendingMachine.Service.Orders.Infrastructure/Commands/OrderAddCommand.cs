using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    public class OrderAddCommand : IRequest<OrderAddResponse>
    {
        public DateTimeOffset OrderDate { get; set; }
        public IEnumerable<OrderProductItemCommand> OrderProducts { get; set; }
        public MachineStatusCommand MachineStatus { get; set; }
    }

    public class OrderAddResponse
    {
        public int OrderId { get; set; }
        public bool CanConfirm { get; set; }
    }
}
