using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Infrastructure.Events;

namespace VendingMachine.Service.Machines.Infrastructure.Handlers
{
    /// https://github.com/jbogard/MediatR/wiki
    public class MachineRequestsHandler :
        IRequestHandler<CreateNewMachineCommand, int>
    {
        private readonly IMediator mediator;

        public MachineRequestsHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<int> Handle(CreateNewMachineCommand request, CancellationToken cancellationToken)
        {
            await mediator.Publish(new NewMachineCreatedEvent()).ConfigureAwait(false);
            throw new System.NotImplementedException();
        }
    }
}
