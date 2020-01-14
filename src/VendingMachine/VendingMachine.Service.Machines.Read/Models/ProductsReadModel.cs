using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class ProductsReadModel : IReadEntity
    {
        public List<ProductReadModel> Products { get; set; } = new List<ProductReadModel>();
    }

    public class ProductReadModel
    {
        public int Id { get; set; }
    }

    public class HistoryProductsReadModel : IReadEntity
    {
        public List<HistoryProductReadModel> Products { get; set; } = new List<HistoryProductReadModel>();
    }

    public class HistoryProductReadModel
    {
        public int Id { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public DateTimeOffset ProvidedDate { get; set; }
    }
}
