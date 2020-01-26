using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        public MachineStatus MachineStatus { get; private set; }
        public ICollection<OrderProductItem> OrderProductItems { get; } = new List<OrderProductItem>();
        public DateTimeOffset OrderDate { get; }
        public Billing Billing { get; private set; }
        public bool CanConfirm => Billing.IsValid;
        public bool Processed { get; private set; }

        /*EF Core - Query CTOR */
        private Order(DateTimeOffset orderDate)
        {
            OrderDate = orderDate;
        }

        public Order(
            MachineStatus machineStatus,
            ICollection<OrderProductItem> orderProductItems, 
            DateTimeOffset orderDate)
        {
            if (!orderProductItems.Any())
                throw new ArgumentException("Order Product Item must be not empty");
            MachineStatus = machineStatus;
            OrderProductItems = orderProductItems.ToArray();
            OrderDate = orderDate;
            CheckMoney();
        }

        public void AddProductToBasket(OrderProductItem productItem)
        {
            OrderProductItems.Add(productItem);
            CheckMoney();
            //DomainEvents.Add()
        }

        public void UpdateMachineStatus(MachineStatus currentMachineStatus)
        {
            MachineStatus = currentMachineStatus;
            CheckMoney();
            //DomainEvents.Add()
        }

        private void CheckMoney()
        {
            Billing = new Billing(MachineStatus.CoinsCurrentSupply, OrderProductItems);
        }
    }
}
