using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Application.Cachings
{
    public static class CachingKeys
    {
        public static string MachineInformationKey(int machineId) => $"MachineInformationKey-{machineId}";
        public static string ProductsInMachineKey(int machineId) => $"ProductsInMachineKey-{machineId}";
    }
}
