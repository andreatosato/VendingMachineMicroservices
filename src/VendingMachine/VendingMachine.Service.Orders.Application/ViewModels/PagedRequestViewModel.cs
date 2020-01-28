using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Orders.Application.ViewModels
{
    public class PagedRequestViewModel
    {
        [FromQuery]
        public int Skip { get; set; }

        [FromQuery]
        public int Take { get; set; }

        public PagedRequest ToReadModel() => new PagedRequest { Take = Take, Skip = Skip };
    }
}
