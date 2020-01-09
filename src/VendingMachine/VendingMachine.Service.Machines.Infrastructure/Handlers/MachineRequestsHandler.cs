using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Infrastructure.Events;
using VendingMachine.Service.Machines.Read;
using VendingMachine.Service.Shared.Exceptions;

namespace VendingMachine.Service.Machines.Infrastructure.Handlers
{
    public class MachineRequestsHandler :
        IRequestHandler<CreateNewMachineCommand, int>,
        IRequestHandler<DeleteMachineCommand, Unit>,
        IRequestHandler<BuyProductsMachineCommand, Unit>,
        IRequestHandler<LoadProductsMachineCommand, Unit>,
        IRequestHandler<SetTemperatureMachineCommand, Unit>,
        IRequestHandler<SetStatusMachineCommand, Unit>,
        IRequestHandler<SetPositionMachineCommand, Unit>
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
            var machineType = new Domain.MachineType(request.Model, (Domain.MachineType.MachineVersion)request.Version);
            var machineItem = new Domain.MachineItem(machineType);
            await machinesUoW.MachineRepository.AddAsync(machineItem).ConfigureAwait(false);
            await machinesUoW.SaveAsync().ConfigureAwait(false);

            await mediator.Publish(new NewMachineCreatedEvent() { Id = machineItem.Id}).ConfigureAwait(false);
            return machineItem.Id;
        }
        public async Task<Unit> Handle(DeleteMachineCommand request, CancellationToken cancellationToken)
        {
            var machineItem = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            if (machineItem == null)
                throw new NotExistsException("Machine not found", request.MachineId);
            await machinesUoW.MachineRepository.DeleteAsync(machineItem).ConfigureAwait(false);
            await machinesUoW.SaveAsync();
            return new Unit();
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
            await machinesUoW.SaveAsync();
            return new Unit();
        }

        public async Task<Unit> Handle(SetTemperatureMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            machine.Temperature = request.Data;
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            logger.LogInformation($"In Machine {request.MachineId}, Set Temperature {request.Data}");
            return new Unit();
        }

        public async Task<Unit> Handle(SetStatusMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            machine.Status = request.Data;
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            logger.LogInformation($"In Machine {request.MachineId}, Set Status {request.Data}");
            return new Unit();
        }

        public async Task<Unit> Handle(SetPositionMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            machine.Position = new Service.Shared.Domain.MapPoint(request.Data.X, request.Data.Y);
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            logger.LogInformation("In Machine {@machineId}, Set Point {@point}", request.MachineId, request.Data);
            return new Unit();
        }
    }
}
