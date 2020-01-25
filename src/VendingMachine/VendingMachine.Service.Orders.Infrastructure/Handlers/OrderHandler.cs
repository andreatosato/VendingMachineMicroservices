using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Orders.Infrastructure.Commands;

namespace VendingMachine.Service.Orders.Infrastructure.Handlers
{
    public class OrderHandler :
        IRequestHandler<OrderAddCommand, OrderAddResponse>
    {
        public async Task<OrderAddResponse> IRequestHandler<OrderAddCommand, OrderAddResponse>.Handle(OrderAddCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
