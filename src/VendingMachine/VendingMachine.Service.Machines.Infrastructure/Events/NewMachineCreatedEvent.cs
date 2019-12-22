using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Events
{
    public class NewMachineCreatedEvent: INotification
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
