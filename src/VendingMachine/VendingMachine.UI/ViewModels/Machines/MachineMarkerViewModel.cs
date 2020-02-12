using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.UI.ViewModels.Machines
{
    public class MachineMarkerViewModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public string Id { get; set; }
    }

    public class PositionViewModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
