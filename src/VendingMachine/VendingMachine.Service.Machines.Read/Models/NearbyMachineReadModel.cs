using Microsoft.SqlServer.Types;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class NearbyMachineReadModel : IReadEntity
    {
        public int Id { get; set; }
        public MapPointReadModel Position { get; set; }
        public void FromDapper(NearbyMachineDapper d, MapPointReadModel position)
        {
            Id = d.Id;
            Position = position;
        }
    }

    public class NearbyMachineDapper
    {
        public int Id { get; set; }
        public SqlGeography Position { get; set; }
    }
}
