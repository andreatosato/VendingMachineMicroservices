using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Infrastructure.Commands
{
    public class ProductDeleteCommand : IRequest
    {
        public int ProductId { get; set; }
    }
}
