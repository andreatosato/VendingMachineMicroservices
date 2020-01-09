using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public abstract class SetDataMachineCommand<T> : IRequest
    {
        public int MachineId { get; set; }
        public T Data { get; set; }
    }

    public class SetTemperatureMachineCommand : SetDataMachineCommand<decimal>
    {
    }

    public class SetStatusMachineCommand : SetDataMachineCommand<bool?>
    {

    }

    public class SetPositionMachineCommand : SetDataMachineCommand<MapPointModel>
    {
    }

    public class MapPointModel
    {
        public MapPointModel()
        {

        }

        public MapPointModel(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
