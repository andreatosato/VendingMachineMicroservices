using Microsoft.SqlServer.Types;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class NearbyMachineReadModel : IReadEntity
    {
        public int Id { get; set; }
        public SqlGeography Position { get; set; }
    }
}
