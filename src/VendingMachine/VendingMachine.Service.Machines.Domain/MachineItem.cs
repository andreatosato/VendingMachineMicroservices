using System;
using System.Collections.Generic;
using VendingMachine.Service.Machines.Domain.DomainEvents;
using VendingMachine.Service.Shared.Domain;

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
        public List<ProductConsumed> HistoryProducts { get; } = new List<ProductConsumed>();
        public List<Product> ActiveProducts { get; } = new List<Product>();
        public DateTimeOffset? LatestLoadedProducts { get; private set; }
        public DateTimeOffset? LatestCleaningMachine { get; private set; }
        public DateTimeOffset? LatestMoneyCollection { get; private set; }
        public decimal CoinsInMachine { get; private set; }
        public decimal CoinsCurrentSupply { get; private set; }

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
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
        }

        protected void BuyProduct(Product productToBuy, DateTimeOffset providedDate)
        {
            var productConsumed = new ProductConsumed(productToBuy);
            productConsumed.SetProvidedDate(providedDate);
            HistoryProducts.Add(productConsumed);
            var productToRemove = ActiveProducts.Find(t => t.Id == productToBuy.Id);
            ActiveProducts.Remove(productToRemove);
        }

        public void LoadProducts(List<Product> productsToLoad, DateTimeOffset? dateToLoad = null)
        {
            var loadProductDate = dateToLoad ?? DateTimeOffset.UtcNow;
            LatestLoadedProducts = loadProductDate;
            foreach (var p in productsToLoad)
            {
                LoadProduct(p, loadProductDate);
            }
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
        }

        protected void LoadProduct(Product productToLoad, DateTimeOffset loadDate)
        {
            if(!productToLoad.IsActive)
                productToLoad.SetActivationDate(loadDate);

            if(ActiveProducts.Find(t => t.Id == productToLoad.Id) == null)
            {
                ActiveProducts.Add(productToLoad);
                HistoryProducts.Add(new ProductConsumed(productToLoad));
            }
            else
            {
                // Product already exists
            }
        }

        //public void AddCoins(decimal coinAdded)
        //{
        //    CoinsInMachine += coinAdded;
        //    CoinsCurrentSupply += coinAdded;
        //    AddDomainEvent(new MachineItemUpdatedEvent(Id));
        //}

        public decimal RestCoins()
        {
            decimal coinsToRest = CoinsCurrentSupply;
            CoinsInMachine -= CoinsCurrentSupply;
            CoinsCurrentSupply = 0;
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
            return coinsToRest;
        }

        // When Buy Products and confirm, we must subtract supply coin to transactions
        public void SupplyCoins(decimal coinsSupply)
        {
            CoinsCurrentSupply -= coinsSupply;
            CoinsInMachine -= coinsSupply;
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
        }

        public void CollectMoney()
        {
            LatestMoneyCollection = DateTimeOffset.UtcNow;
            CoinsInMachine = 0;
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
        }

        public void CleanMachine()
        {
            LatestCleaningMachine = DateTimeOffset.UtcNow;
            AddDomainEvent(new MachineItemUpdatedEvent(Id));
        }
    }
}
