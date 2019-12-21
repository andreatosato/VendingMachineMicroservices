using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Data.Seeds
{
    public static class SeedBase
    {
        public static Point SetPosition(decimal x, decimal y)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            return geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate((double)x, (double)y));
        }
    }
}
