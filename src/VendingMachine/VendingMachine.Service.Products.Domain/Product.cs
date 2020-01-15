using System;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Products.Domain
{
    public abstract class Product : Entity, IAggregateRoot
    {
        public GrossPrice Price { get; private set; }
        public string Name { get; }

        public Product(string name)
        {
            Name = name;
        }

        public void SetPrice(GrossPrice price)
        {
            Price = price;
        }
    }
}
