using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Products.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public GrossPriceEntity Price { get; set; }
        public string Name { get; set; }
        public byte[] Version { get; set; }
    }
}
