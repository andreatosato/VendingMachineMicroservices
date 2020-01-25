using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Orders.Domain
{
    public class Billing : ValueObject
    {
        public decimal Coins { get; }
        public IEnumerable<OrderProductItem> OrderProductItems { get; }
        public decimal Rest => Coins - Cost;
        public decimal Cost { get; private set; }
        public bool IsValid { get; private set; }

        public Billing(decimal coins, IEnumerable<OrderProductItem> orderProductItems)
        {
            Coins = coins;
            OrderProductItems = orderProductItems;
            CalculateRest();
        }

        private void CalculateRest()
        {
            decimal totalCost = 0;
            using var productEnumerator = OrderProductItems.GetEnumerator();
            while (productEnumerator.MoveNext())
            {
                totalCost += productEnumerator.Current.Price.Value;
            }

            Cost = totalCost;
            IsValid = Cost >= Coins;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Coins;
            yield return OrderProductItems;
        }

        public override string ToString()
        {
            StringBuilder billingStatus = new StringBuilder($"Paid: {Coins} for: ");
            foreach (var p in OrderProductItems)
            {
                billingStatus.AppendFormat("Product Item: {0}. Gross Price: {1}", p.ProductItemId, p.Price.Value);
            }
            return billingStatus.ToString();
        }
    }
}
