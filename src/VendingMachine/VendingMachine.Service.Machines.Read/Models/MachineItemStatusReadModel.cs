using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class MachineItemStatusReadModel : IReadEntity
    {
        public int MachineItemId { get; set; }
        public decimal CoinsCurrentSupply { get; set; }
    }
}
