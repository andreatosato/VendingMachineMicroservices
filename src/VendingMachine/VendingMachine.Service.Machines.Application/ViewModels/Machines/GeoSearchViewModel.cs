using Microsoft.AspNetCore.Mvc;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class GeoSearchViewModel
    {
        [FromQuery]
        public double Latutide { get; set; }
        [FromQuery]
        public double Longitude { get; set; }

        /// <summary>
        /// Metres value
        /// </summary>
        [FromQuery]
        public decimal? Radius { get; set; } = 50;
    }
}
