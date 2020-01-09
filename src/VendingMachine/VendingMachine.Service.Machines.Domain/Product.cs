using System;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines.Domain
{

    // Table Per Type in EntityFramework not work properly with DDD.
    // Solution? Duplicate logic
    public class Product : Entity, IAggregateRoot
    {
        public DateTimeOffset? ActivationDate { get; protected set; }
       
        public bool IsActive => ActivationDate.HasValue;
        

        public Product(int id, DateTimeOffset? activationDate = null)
        {
            Id = id;
            ActivationDate = activationDate;
        }

        protected Product()
        {
        }

        public void SetActivationDate(DateTimeOffset activationDate)
        {
            ActivationDate = activationDate;
        }
    }

    public class ProductConsumed : Entity, IAggregateRoot
    {
        public DateTimeOffset? ProvidedDate { get; private set; }
        public DateTimeOffset? ActivationDate { get; protected set; }

        public bool IsActive => ActivationDate.HasValue;


        public ProductConsumed(int id, DateTimeOffset? activationDate, DateTimeOffset? providedDate)
        {
            Id = id;
            ActivationDate = activationDate;
            ProvidedDate = providedDate;
        }

        public ProductConsumed(Product baseProduct)
        {
            Id = baseProduct.Id;
            SetActivationDate(baseProduct.ActivationDate.GetValueOrDefault());        
        }

        protected ProductConsumed()
        {

        }

        private void SetActivationDate(DateTimeOffset activationDate)
        {
            ActivationDate = activationDate;
        }

        public void SetProvidedDate(DateTimeOffset providedDate)
        {
            ProvidedDate = providedDate;
        }
    }
}
