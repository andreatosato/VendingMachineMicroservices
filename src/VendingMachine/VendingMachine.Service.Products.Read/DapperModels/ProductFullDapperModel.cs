using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Products.Read.DapperModels
{
    public class ProductFullDapperModel
    {
        public int Id { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public int TaxPercentage { get; set; }
        public decimal Rate { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; set; }
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Litre { get; set; }
        public decimal Grams { get; set; }

        public ProductReadModel ToReadModel()
        {
            if (Discriminator == "ColdDrinkEntity")
                return new ColdDrinkReadModel
                {
                    Id = this.Id,
                    Litre = this.Litre,
                    Name = this.Name,
                    Price = new GrossPriceReadModel
                    {
                        GrossPrice = this.GrossPrice,
                        NetPrice = this.NetPrice,
                        Rate = this.Rate,
                        TaxPercentage = this.TaxPercentage
                    },
                    TemperatureMaximum = this.TemperatureMaximum,
                    TemperatureMinimum = this.TemperatureMinimum
                };

            if (Discriminator == "HotDrinkEntity")
                return new HotDrinkReadModel
                {
                    Id = this.Id,
                    Grams = this.Grams,
                    Name = this.Name,
                    Price = new GrossPriceReadModel
                    {
                        GrossPrice = this.GrossPrice,
                        NetPrice = this.NetPrice,
                        Rate = this.Rate,
                        TaxPercentage = this.TaxPercentage
                    },
                    TemperatureMaximum = this.TemperatureMaximum,
                    TemperatureMinimum = this.TemperatureMinimum
                };

            if (Discriminator == "SnackEntity")
                return new SnackReadModel
                {
                    Id = this.Id,
                    Grams = this.Grams,
                    Name = this.Name,
                    Price = new GrossPriceReadModel
                    {
                        GrossPrice = this.GrossPrice,
                        NetPrice = this.NetPrice,
                        Rate = this.Rate,
                        TaxPercentage = this.TaxPercentage
                    }
                };
            throw new System.InvalidOperationException("No entity on database");
        }
    }
}
