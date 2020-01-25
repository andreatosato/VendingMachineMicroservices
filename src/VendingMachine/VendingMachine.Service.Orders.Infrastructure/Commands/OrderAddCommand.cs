using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    public class OrderAddCommand : IRequest<OrderAddResponse>
    {
    }

    public class OrderAddResponse
    {
        public int OrderId { get; set; }
    }
}
