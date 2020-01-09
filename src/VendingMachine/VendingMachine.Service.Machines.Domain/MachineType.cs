using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines.Domain
{
    public class MachineType : Entity, IAggregateRoot
    {
        public string Model { get; }
        public MachineVersion Version { get; }
        public MachineType(string model, MachineVersion version)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentNullException("Machine type model must be a value");
            }
            Model = model;
            Version = version;
        }
        public enum MachineVersion
        {
            Coffee,
            Frigo,
            FrigoAndCoffee
        }
    }
}
