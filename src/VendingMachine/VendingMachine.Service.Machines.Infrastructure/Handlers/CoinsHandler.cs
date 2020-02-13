using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Infrastructure.Commands;
using VendingMachine.Service.Machines.Infrastructure.Events;

namespace VendingMachine.Service.Machines.Infrastructure.Handlers
{
    public class CoinsHandler :
        IRequestHandler<AddCoinsMachineCommand, Unit>,
        IRequestHandler<ResetCoinsMachineCommand, ResetCoinsMachineResult>,
        IRequestHandler<CollectCoinsMachineCommand, CollectCoinsMachineResult>,
        IRequestHandler<RequestRestMachineCommand, decimal>,
        INotificationHandler<CoinNotificationEvent>
    {
        private readonly IMediator mediator;
        private readonly IMachinesUoW machinesUoW;

        public CoinsHandler(IMediator mediator, IMachinesUoW machinesUoW)
        {
            this.mediator = mediator;
            this.machinesUoW = machinesUoW;
        }

        #region Commands
        // Add coins in machine
        public async Task<Unit> Handle(AddCoinsMachineCommand request, CancellationToken cancellationToken)
        {
            //var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            //machine.AddCoins(request.CoinsAdded);
            //await machinesUoW.SaveAsync().ConfigureAwait(false);

            //await mediator
            //    .Publish(new CoinNotificationEvent(request.MachineId, request.CoinsAdded, CoinOperation.Add))
            //    .ConfigureAwait(false);
            return new Unit();
        }

        // Reset inserted coins.
        public async Task<ResetCoinsMachineResult> Handle(ResetCoinsMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            var restCoins = machine.RestCoins();
            await machinesUoW.SaveAsync().ConfigureAwait(false);

            await mediator
                .Publish(new CoinNotificationEvent(request.MachineId, restCoins, CoinOperation.Subtract))
                .ConfigureAwait(false);
            return new ResetCoinsMachineResult()
            {
                RestCoins = restCoins
            }; ;
        }

        // Collect all coin in machine
        public async Task<CollectCoinsMachineResult> Handle(CollectCoinsMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            CollectCoinsMachineResult result = new CollectCoinsMachineResult()
            {
                CoinsCollected = machine.CoinsInMachine
            };
            machine.CollectMoney();
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<decimal> Handle(RequestRestMachineCommand request, CancellationToken cancellationToken)
        {
            var machine = await machinesUoW.MachineRepository.FindAsync(request.MachineId).ConfigureAwait(false);
            decimal rest = machine.RestCoins();
            await machinesUoW.SaveAsync().ConfigureAwait(false);
            return rest;
        }

        #endregion

        #region Events
        public async Task Handle(CoinNotificationEvent notification, CancellationToken cancellationToken)
        {
            // Send Payload to SignalR
            switch (notification.Operation)
            {
                case CoinOperation.Add:
                    break;
                case CoinOperation.Subtract:
                    break;
                case CoinOperation.Collect:
                    break;
            }
            await Task.FromResult(1);
        }
        #endregion
    }
}