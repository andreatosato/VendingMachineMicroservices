using Microsoft.SqlServer.Types;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class NearbyMachine
    {
        public int Id { get; set; }
        public SqlGeography Position { get; set; }
    }
}
