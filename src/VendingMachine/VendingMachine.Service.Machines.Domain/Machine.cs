using System;
using System.Runtime.CompilerServices;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Domain
{
    public class Machine : Entity, IAggregateRoot
    {
        private readonly DateTime _dataCreated;
        private DateTime? _dataUpdated;

        public MapPoint Position { get; set; }
        public decimal? Temperature { get; set; }
        // Start or Stop or Not Set
        public bool? Status { get; set; }
        public MachineType MachineType { get; private set; }

        public Machine(MachineType machineType)
            : this()
        {
            MachineType = machineType;
        }

        protected Machine()
        {
            _dataCreated = DateTime.UtcNow;
        }
    }
}
