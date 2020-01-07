using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class RequestRestViewModel
    {
        public int MachineId { get; set; }
        public decimal Rest { get; set; }
    }

    public class ResponseRestErrorViewModel
    {
        public decimal Difference { get; set; }
        public RestErrorType ErrorType { get; set; }
    }

    public enum RestErrorType
    {
        MoreCoinsInMachine,
        LessCoinsInMachine
    }
}
