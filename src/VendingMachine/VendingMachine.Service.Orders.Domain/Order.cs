using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Service.Orders.Domain.DomainEvents;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        public MachineStatus MachineStatus { get; private set; }
        public ICollection<OrderProductItem> OrderProductItems { get; } = new List<OrderProductItem>();
        public DateTimeOffset OrderDate { get; }
        public Billing Billing { get; private set; }
        public bool CanConfirm => Billing.IsValid & !Processed;
        public bool Processed => Confirmed || Cancelled;
        public bool Confirmed { get; private set; }
        public bool Cancelled { get; private set; }

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
            foreach (var pi in orderProductItems)   AddProductToBasket(pi, false);            
            OrderDate = orderDate;
            CheckMoney();
        }
        
        // Only for Ctor
        public void AddProductToBasket(OrderProductItem productItem)
        {
            AddProductToBasket(productItem, true);
        }

        private void AddProductToBasket(OrderProductItem productItem, bool checkMoney)
        {
            OrderProductItems.Add(productItem);
            if(checkMoney)
                CheckMoney();
            AddDomainEvent(new OrderProductToBasketEvent()
            {
                Operation = OrderProductToBasketEvent.OperationType.Add,
                OrderId = Id,
                ProductItemId = productItem.ProductItemId
            });
        }

        public void RemoveProductToBasket(int productItemId)
        {
            var productToRemove = OrderProductItems.FirstOrDefault(x => x.ProductItemId == productItemId);
            if(productToRemove != null)
            {
                OrderProductItems.Remove(productToRemove);
                CheckMoney();
                AddDomainEvent(new OrderProductToBasketEvent()
                {
                    Operation = OrderProductToBasketEvent.OperationType.Remove,
                    OrderId = Id,
                    ProductItemId = productToRemove.ProductItemId
                });
            }
        }

        public void UpdateMachineStatus(MachineStatus currentMachineStatus)
        {
            if(MachineStatus.MachineId == currentMachineStatus.MachineId)
            {
                MachineStatus = currentMachineStatus;
                CheckMoney();
                //AddDomainEvent
            }
            // Errore, MachineId not correct
            //DomainEvents.Add()
        }

        public void ConfirmOrder()
        {
            Confirmed = true;
        }

        public void CancelOrder()
        {
            Cancelled = true;
        }

        private void CheckMoney()
        {
            Billing = new Billing(MachineStatus.CoinsCurrentSupply, OrderProductItems);
        }
    }
}
