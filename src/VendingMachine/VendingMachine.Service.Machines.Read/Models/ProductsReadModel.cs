using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class ProductsReadModel : IReadEntity
    {
        public List<ProductReadModel> Products { get; set; }
    }

    public class ProductReadModel
    {
        public int Id { get; set; }
    }
}
