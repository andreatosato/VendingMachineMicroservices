using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Application.ViewModels
{
    public class OrderAddViewModel
    {
        public MachineStatusViewModel ModelStatus { get; set; }
        public ICollection<OrderProductItemViewModel> ProductItems { get; set; }
    }

    public class OrderAddedViewModel 
    {
        public int OrderId { get; set; }
        public bool CanConfirm { get; set; }
        //public decimal MoneyToBeProvided { get; set; }
    }
}
