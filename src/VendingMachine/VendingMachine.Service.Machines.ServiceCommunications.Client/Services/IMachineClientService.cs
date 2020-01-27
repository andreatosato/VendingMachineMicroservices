using System.Threading.Tasks;

namespace VendingMachine.Service.Machines.ServiceCommunications.Client.Services
{
    public interface IMachineClientService
    {
        Task<bool> ExistMachineAsync(int machineId);
        Task<GetMachineInfoResponse> GetMachineInfoAsync(int machineId);
    }
}
