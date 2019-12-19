using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Services.Shared.Domain
{
    public class MapPoint
    {
        public decimal X { get; }
        public decimal Y { get; }
        public MapPoint(decimal x, decimal y) => (X, Y) = (x, y);
    }
}
