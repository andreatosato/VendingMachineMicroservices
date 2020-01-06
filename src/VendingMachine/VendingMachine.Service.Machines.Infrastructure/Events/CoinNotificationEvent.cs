using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Events
{
    public class CoinNotificationEvent : INotification
    {
        public CoinNotificationEvent(int machineId, decimal coins, CoinOperation operation)
        {
            MachineId = machineId;
            Coins = coins;
            Operation = operation;
        }
        public int MachineId { get; }
        public decimal Coins { get; }
        public CoinOperation Operation { get; }
    }

    public enum CoinOperation
    {
        Add,
        Subtract,
        Collect,
        Sell
    }
}
