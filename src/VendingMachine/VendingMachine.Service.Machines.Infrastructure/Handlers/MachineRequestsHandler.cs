using MediatR;
using Microsoft.Extensions.Logging;
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
        IRequestHandler<BuyProductsMachineCommand, Unit>,
        IRequestHandler<LoadProductsMachineCommand, Unit>
    {
        private readonly IMediator mediator;
        private readonly IMachinesUoW machinesUoW;
        private readonly ILogger logger;

        public MachineRequestsHandler(IMediator mediator, IMachinesUoW machinesUoW, ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.machinesUoW = machinesUoW;
            this.logger = loggerFactory.CreateLogger(typeof(MachineRequestsHandler));
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

            if (machine.CoinsCurrentSupply != request.TotalRest)
                throw new InvalidOperationException("Coins in machine isn't equals to coins to rest.");

            await machinesUoW.MachineRepository.UpdateAsync(machine).ConfigureAwait(false);
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            return new Unit();
        }

        public async Task<Unit> Handle(LoadProductsMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            DateTimeOffset activationDate = DateTimeOffset.UtcNow;
            List<Domain.Product> products = new List<Domain.Product>();
            foreach (var p in request.Products)
            {
                // Check Products Microservice that Product EXISTS!
                // TODO
                products.Add(new Domain.Product(p, activationDate));
                logger.LogInformation($"Load product: {p} in machine {request.MachineId}");
            }

            machine.LoadProducts(products, activationDate);
            //await machinesUoW.MachineRepository.UpdateAsync(machine).ConfigureAwait(false);
            await machinesUoW.SaveAsync();
            return new Unit();
        }
    }
}
