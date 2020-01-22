using System;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class CreateMachineItemViewModel
    {
        public int Id { get; set; }
        public decimal? Temperature { get; set; }
        public bool? Status { get; set; }
        public MapPointViewModel Position { get; set; }
        public MachineTypeViewModel Model { get; set; }
    }

    public class UpdateMachineItemViewModel : CreateMachineItemViewModel
    {
        public int Id { get; set; }
    }

    public class MachineTypeViewModel
    {
        public string ModelName { get; set; }
        public short Version { get; set; }
    }

    public class MapPointViewModel
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }


    public class ProductItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public decimal? SoldPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTimeOffset? Purchased { get; set; }
        public DateTimeOffset? Sold { get; set; }
    }

    public class ProductViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public GrossPriceViewModel Price { get; set; }
    }

    public class GrossPriceViewModel
    {
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
