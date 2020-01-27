using System.Threading.Tasks;
using static VendingMachine.Service.Machines.ServiceCommunications.MachineItems;

namespace VendingMachine.Service.Machines.ServiceCommunications.Client.Services
{
    public class MachineClientService : IMachineClientService
    {
        private readonly MachineItemsClient client;
        public MachineClientService(MachineItemsClient client)
        {
            this.client = client;
        }

        public async Task<bool> ExistMachineAsync(int machineId)
        {
            ExistMachineRequest request = new ExistMachineRequest() { MachineId = machineId };
            ExistMachineResponse response = await client.ExistMachineAsync(request);
            return response.Exist;
        }

        public async Task<GetMachineInfoResponse> GetMachineInfoAsync(int machineId)
        {
            GetMachineInfoRequest request = new GetMachineInfoRequest()
            {
                MachineId = machineId
            };

            return await client.GetMachineInfosAsync(request);
        }
    }
}
