using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Events
{
    public class NewMachineCreatedEvent: INotification
    {
        public int Id { get; set; }
    }
}
