using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Domain.DomainEvents;

namespace VendingMachine.Service.Products.Infrastructure.Handlers
{
    public class ProductItemHandler :
        //IRequestHandler<SetPositionMachineCommand, Unit>,
        INotificationHandler<ProductItemExpirationDateEvent>
    {
        public async Task Handle(ProductItemExpirationDateEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
