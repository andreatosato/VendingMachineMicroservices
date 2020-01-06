using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Infrastructure.Events;
using VendingMachine.Service.Machines.Read;

namespace VendingMachine.Service.Machines.Infrastructure.Handlers
{
    /// https://github.com/jbogard/MediatR/wiki
    public class MachineRequestsHandler :
        IRequestHandler<CreateNewMachineCommand, int>,
        IRequestHandler<BuyProductsMachineCommand, Unit>
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

        public async Task<Unit> Handle(BuyProductsMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            List<Domain.Product> products = new List<Domain.Product>();
            foreach (var p in request.ProductsBuy)
            {
                var currentProduct = machine.ActiveProducts.Find(x => x.Id == p);
                if (currentProduct == null)
                    throw new ArgumentException($"Product id: {p} not found in machine {machine.Id}");

                products.Add(currentProduct);
            }
            // Acquisto gli articoli
            machine.BuyProducts(products);
            // Prelevo la moneta
            machine.SupplyCoins(request.TotalBuy);
            await mediator
              .Publish(new CoinNotificationEvent(request.MachineId, request.TotalBuy, CoinOperation.Sell))
              .ConfigureAwait(false);
            // Ritorno il resto
            var restCoins = machine.RestCoins();
            await mediator
               .Publish(new CoinNotificationEvent(request.MachineId, restCoins, CoinOperation.Subtract))
               .ConfigureAwait(false);

            await machinesUoW.MachineRepository.UpdateAsync(machine).ConfigureAwait(false);
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            return new Unit();
        }
    }
}
