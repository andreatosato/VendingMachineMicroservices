using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class CreateMachineItemViewModel
    {
        public decimal? Temperature { get; set; }
        public bool? Status { get; set; }
        public MapPointViewModel Position { get; set; }
        public MachineTypeViewModel Model { get; set; }
    }

    public class UpdateMachineItemViewModel : CreateMachineItemViewModel
    {
        public int Id { get; set; }
    }

    public class MachineTypeViewModel
    {
        public string Model { get; }
        public MachineVersion Version { get; }
        public enum MachineVersion
        {
            Coffee,
            Frigo,
            FrigoAndCoffee
        }
    }

    public class MapPointViewModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
