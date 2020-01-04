using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Services.Shared.Domain;

namespace VendingMachine.Service.Machines.Domain
{
    public class Product : Entity, IAggregateRoot
    {
        public DateTimeOffset? ActivationDate { get; protected set; }
       
        public bool IsActive => ActivationDate.HasValue;
        

        public Product(DateTimeOffset? activationDate)
        {
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


        public ProductConsumed(DateTimeOffset? activationDate, DateTimeOffset? providedDate)
        {
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
        public void SetActivationDate(DateTimeOffset activationDate)
        {
            ActivationDate = activationDate;
        }

        public void SetProvidedDate(DateTimeOffset providedDate)
        {
            ProvidedDate = providedDate;
        }
    }
}
