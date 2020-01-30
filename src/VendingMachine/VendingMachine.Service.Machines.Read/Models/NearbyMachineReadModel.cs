using Microsoft.SqlServer.Types;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class NearbyMachineReadModel : IReadEntity
    {
        public int Id { get; set; }
        public MapPointReadModel Position { get; set; }
        public decimal Distance { get; set; }

        public static NearbyMachineReadModel FromDapper(NearbyMachineDapper d)
        {
            return new NearbyMachineReadModel
            {
                Id = d.Id,
                Position = new MapPointReadModel(d.Position.Lat.Value, d.Position.Long.Value),
                Distance = d.Distance
            };
        }
    }

    public class NearbyMachineDapper
    {
        public int Id { get; set; }
        public SqlGeography Position { get; set; }
        public decimal Distance { get; set; }
    }

    public static class GeograpyUtility
    {
        private const int SRID = 4326;
        public static SqlGeography GetPointSqlGeography(double latitude, double longitude)
        {
            return SqlGeography.Point(latitude, longitude, SRID);
        }
    }
}
