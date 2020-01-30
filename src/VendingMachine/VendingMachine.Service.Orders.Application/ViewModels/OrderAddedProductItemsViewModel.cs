using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Application.ViewModels
{
    public class OrderUpdateProductItemViewModel
    {
        [FromRoute]
        public int OrderId { get; set; }
        [FromRoute]
        public int ProductItem { get; set; }
    }

    public class OrderUpdateProductItemsViewModel
    {
        [FromRoute]
        public int OrderId { get; set; }

        [FromBody]
        public ICollection<int> ProductItems { get; set; }
    }
}
