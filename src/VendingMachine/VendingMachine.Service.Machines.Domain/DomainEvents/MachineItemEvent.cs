using MediatR;

namespace VendingMachine.Service.Machines.Domain.DomainEvents
{
    public class MachineItemCreatedEvent: INotification
    {
        public int Id { get; set; }
    }

    public class MachineItemUpdatedEvent : INotification
    {
        public MachineItemUpdatedEvent(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
