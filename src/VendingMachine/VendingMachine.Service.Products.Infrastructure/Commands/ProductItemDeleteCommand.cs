using MediatR;

namespace VendingMachine.Service.Products.Infrastructure.Commands
{
    public class ProductItemDeleteCommand : IRequest
    {
        public int ProductItemId { get; set; }
    }
}
