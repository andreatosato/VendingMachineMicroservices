
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace VendingMachine.Service.Products.Read.Models
{
    public class ProductsReadModel
    {
        public Collection<ProductReadModel> Products { get; set; } = new Collection<ProductReadModel>();
    }

    public class ProductReadModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public GrossPriceReadModel Price { get; set; }
        // Anzichè utilizzare la via classica https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to#support-polymorphic-deserialization

        public string NameOf { get; set; }
    }

    public class GrossPriceReadModel
    {
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public int TaxPercentage { get; set; }
        public decimal Rate { get; set; }
    }

    
    public class ColdDrinkReadModel : ProductReadModel
    {
        public ColdDrinkReadModel()
        {
            NameOf = nameof(ColdDrinkReadModel);
        }

        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Litre { get; set; }
    }

    public class HotDrinkReadModel : ProductReadModel
    {
        public HotDrinkReadModel()
        {
            NameOf = nameof(HotDrinkReadModel);
        }
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Grams { get; set; }
    }

    public class SnackReadModel : ProductReadModel
    {
        public SnackReadModel()
        {
            NameOf = nameof(SnackReadModel);
        }

        public decimal Grams { get; set; }
    }
}
