using MediatR;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    public class OrderConfirmCommand : IRequest<OrderConfirmResponse>
    {
        public int OrderId { get; set; }
    }

    public class OrderConfirmResponse
    {
        public bool Confirmed { get; set; }
    }
}
