using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Domain.DomainEvents
{
    public class ProductItemExpirationDateEvent : INotification
    {
        public int ProductItemId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
