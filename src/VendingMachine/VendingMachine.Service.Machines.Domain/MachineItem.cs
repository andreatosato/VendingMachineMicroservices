using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Domain
{
    public class MachineItem : Entity, IAggregateRoot
    {
        private readonly DateTime _dataCreated;
        private readonly DateTime? _dataUpdated;

        public MapPoint Position { get; set; }
        public decimal? Temperature { get; set; }
        // Start or Stop or Not Set
        public bool? Status { get; set; }
        public MachineType MachineType { get; private set; }

        public decimal MoneyFromBirth { get; }
        public decimal MoneyMonth { get; }
        public decimal MoneyYear { get; }
        public List<ProductConsumed> HistoryProducts { get; } = new List<ProductConsumed>();
        public List<Product> ActiveProducts { get; } = new List<Product>();
        public DateTimeOffset? LatestLoadedProducts { get; private set; }
        public DateTimeOffset? LatestCleaningMachine { get; private set; }
        public DateTimeOffset? LatestMoneyCollection { get; private set; }
        public decimal CoinsInMachine { get; private set; }

        public MachineItem(MachineType machineType)
            : this()
        {
            MachineType = machineType;
        }

        // Testing only
        protected MachineItem()
        {
            _dataCreated = DateTime.UtcNow;
        }

        /// <summary>
        /// Update quantity in machine, no money, no rest. Only products in machine data.
        /// </summary>
        /// <param name="productToBuy"></param>
        public void BuyProducts(List<Product> productToBuy)
        {
            DateTimeOffset providedDate = DateTimeOffset.UtcNow;
            foreach (var p in productToBuy)
            {
                BuyProduct(p, providedDate);
            }
        }

        protected void BuyProduct(Product productToBuy, DateTimeOffset providedDate)
        {
            var productConsumed = new ProductConsumed(productToBuy);
            productConsumed.SetProvidedDate(providedDate);
            HistoryProducts.Add(productConsumed);
            var productToRemove = ActiveProducts.Find(t => t.Id == productToBuy.Id);
            ActiveProducts.Remove(productToRemove);
        }

        public void LoadProducts(List<Product> productsToLoad)
        {
            var loadProductDate = DateTimeOffset.UtcNow;
            LatestLoadedProducts = loadProductDate;
            foreach (var p in productsToLoad)
            {
                LoadProduct(p, loadProductDate);
            }
        }

        protected void LoadProduct(Product productToLoad, DateTimeOffset loadDate)
        {
            productToLoad.SetActivationDate(loadDate);
            ActiveProducts.Add(productToLoad);
            HistoryProducts.Add(new ProductConsumed(productToLoad));
        }

        public void AddCoin(decimal coinAdded)
        {
            CoinsInMachine += coinAdded;
        }

        public void RestCoint(decimal coinSubtract)
        {
            CoinsInMachine -= coinSubtract;
        }

        public void CollectMoney()
        {
            LatestMoneyCollection = DateTimeOffset.UtcNow;
            CoinsInMachine = 0;
        }

        public void CleanMachine()
        {
            LatestCleaningMachine = DateTimeOffset.UtcNow;
        }
    }
}
