using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Domain
{
    public class MachineStatus : ValueObject
    {
        public MachineStatus(int machineId, decimal coinsCurrentSupply)
        {
            if (machineId <= 0)
                throw new ArgumentException("Machine Id must be not empty");
            MachineId = machineId;
            CoinsCurrentSupply = coinsCurrentSupply;
        }

        public int MachineId { get; }
        public decimal CoinsCurrentSupply { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MachineId;
            yield return CoinsCurrentSupply;
        }
    }
}
