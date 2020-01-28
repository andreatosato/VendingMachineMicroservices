using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Orders.Read.Models
{
    public class OrdersReadModel : IReadPagedEntity<OrderReadModel>
    {
        public ICollection<OrderReadModel> Entities { get; set; } = new List<OrderReadModel>();
        public int CurrentItem { get; set; }
        public int Total { get; set; }
    }

    public class OrderReadModel : IReadEntity
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public decimal CoinsCurrentSupply { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public bool Processed { get; set; }
        public ICollection<OrderProductItemReadModel> OrderProductItems { get; set; } = new List<OrderProductItemReadModel>();        
    }

    public class OrderProductItemReadModel
    {
        public int ProductItemId { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal Rate { get; set; }
    }
}
