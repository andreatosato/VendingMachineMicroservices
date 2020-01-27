using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.ServiceCommunications.Client.Services;
using VendingMachine.Service.Orders.Application.ViewModels;

namespace VendingMachine.Service.Orders.Application.Validations
{
    public class MachineStatusValidation : AbstractValidator<MachineStatusViewModel>
    {
        private readonly IMachineClientService machineClient;

        public MachineStatusValidation(IMachineClientService machineClient)
        {
            this.machineClient = machineClient ?? throw new System.ArgumentNullException(nameof(IMachineClientService));

            RuleFor(x => x.MachineId).GreaterThanOrEqualTo(0).MustAsync(CheckMachineExist);
            RuleFor(c => c.CoinCurrentSupply).GreaterThanOrEqualTo(0);
        }

        public async Task<bool> CheckMachineExist(int machineId, CancellationToken token)
        {
            return await machineClient.ExistMachineAsync(machineId).ConfigureAwait(false);
        }
    }
}
