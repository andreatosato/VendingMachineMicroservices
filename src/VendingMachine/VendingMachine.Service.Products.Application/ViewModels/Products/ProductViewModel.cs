namespace VendingMachine.Service.Products.Application.ViewModels.Products
{
    public abstract class ProductViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public GrossPriceViewModel Price { get; set; }
    }

    public class GrossPriceViewModel
    {
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public int TaxPercentage { get; set; }
        public decimal Rate { get; set; }
    }

    public class ColdDrinkViewModel : ProductViewModel
    {
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Litre { get; set; }
    }

    public class HotDrinkViewModel : ProductViewModel
    {
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Grams { get; set; }
    }

    public class SnackViewModel : ProductViewModel
    {
        public decimal Grams { get; set; }
    }
}
