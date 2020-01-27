using MediatR;

namespace VendingMachine.Service.Orders.Domain.DomainEvents
{
    public class OrderProductToBasketEvent : INotification
    {
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
        public OperationType Operation { get; set; }

        public enum OperationType
        {
            Add,
            Remove
        }
    }
}
