using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public abstract class SetDataValueViewModel<T>
    {
        public T Data { get; set; }
    }

    public sealed class SetTemperatureViewModel : SetDataValueViewModel<decimal> { }

    public sealed class SetStatusViewModel : SetDataValueViewModel<bool?> { }

    public sealed class SetPositionViewModel : SetDataValueViewModel<MapPointViewModel> { }
}
