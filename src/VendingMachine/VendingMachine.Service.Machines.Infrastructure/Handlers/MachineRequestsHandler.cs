using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Infrastructure.Events;

namespace VendingMachine.Service.Machines.Infrastructure.Handlers
{
    /// https://github.com/jbogard/MediatR/wiki
    public class MachineRequestsHandler :
        AsyncRequestHandler<AddCoinsMachineCommand>,
        IRequestHandler<CreateNewMachineCommand, int>        
    {
        private readonly IMediator mediator;
        private readonly IMachinesUoW machinesUoW;

        public MachineRequestsHandler(IMediator mediator, IMachinesUoW machinesUoW)
        {
            this.mediator = mediator;
            this.machinesUoW = machinesUoW;
        }

        public async Task<int> Handle(CreateNewMachineCommand request, CancellationToken cancellationToken)
        {
            await mediator.Publish(new NewMachineCreatedEvent()).ConfigureAwait(false);
            throw new System.NotImplementedException();
        }

        protected override async Task Handle(AddCoinsMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            machine.AddCoins(request.CoinsAdded);
            await machinesUoW.SaveAsync().ConfigureAwait(false);
        }
    }
}
