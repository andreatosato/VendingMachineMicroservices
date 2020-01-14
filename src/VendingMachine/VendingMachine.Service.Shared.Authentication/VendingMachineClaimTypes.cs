using System;

namespace VendingMachine.Service.Shared.Authentication
{

    public static class VendingMachineClaimTypes
    {
        public const string ApiClaim = "VendingMachineApiClaim";
    }

    public static class VendingMachineClaimValues
    {
        public const string MachineApi = "Machine.Api";
        public const string ProductApi = "Product.Api";
        public const string OrderApi = "Order.Api";
    }
}
