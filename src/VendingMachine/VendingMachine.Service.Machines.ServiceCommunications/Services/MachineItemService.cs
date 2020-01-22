using Grpc.Core;
using System;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.ServiceCommunications.Services
{
    public class MachineItemService : MachineItems.MachineItemsBase
    {
        private readonly IMachineQuery machineQuery;

        public MachineItemService(IMachineQuery machineQuery)
        {
            this.machineQuery = machineQuery;
        }

        public override async Task<GetMachineInfoResponse> GetMachineInfos(GetMachineInfoRequest request, ServerCallContext context)
        {
            if (request.MachineId > 0)
            {
                var result = await machineQuery.GetMachineItemInfoAsync(request.MachineId).ConfigureAwait(false);
                GetMachineInfoResponse response = new GetMachineInfoResponse().ToResponse(result);
                return response;
            }

            throw new ArgumentNullException();
        }

        public override async Task<ExistMachineResponse> ExistMachine(ExistMachineRequest request, ServerCallContext context)
        {
            var exist = await machineQuery.CheckMachineItemExistsAsync(request.MachineId).ConfigureAwait(false);
            return new ExistMachineResponse { Exist = exist };
        }
    }
}
