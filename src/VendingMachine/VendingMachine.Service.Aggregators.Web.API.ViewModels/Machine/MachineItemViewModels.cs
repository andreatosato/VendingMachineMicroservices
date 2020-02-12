using System;
using System.Collections.Generic;
using VendingMachine.Service.Products.ServiceCommunications;

namespace VendingMachine.Service.Aggregators.Web.API.ViewModels.Machine
{
    public class MachineItemViewModels
    {
        public int Id { get; set; }
        public decimal? Temperature { get; set; }
        public bool? Status { get; set; }
        public decimal? CoinsCurrentSupply { get; set; }
        public decimal? CoinsInMachines { get; set; }
        public decimal MoneyFromBirth { get; set; }
        public decimal MoneyMonth { get; set; }
        public decimal MoneyYear { get; set; }
        public DateTimeOffset? LatestCleaningMachine { get; set; }
        public DateTimeOffset? LatestLoadedProducts { get; set; }
        public DateTimeOffset? LatestMoneyCollection { get; set; }
        public MapPointViewModel Position { get; set; }
        public MachineTypeViewModel MachineType { get; set; }
        public IEnumerable<ProductItemViewModel> ProductItem { get; set; }

        public static MachineItemViewModels ToViewModel(MachineServiceModel machine, List<ProductItemsServiceModel> products)
        {
            List<ProductItemViewModel> productsVM = new List<ProductItemViewModel>();
            products.ForEach(t =>
            {
                ProductItemViewModel productItem = new ProductItemViewModel() 
                {
                    ProductItemId = t.Id,
                    ExpirationDate = t.ExpirationDate?.ToDateTime(),
                    Purchased = t.Purchased?.ToDateTimeOffset(),
                    Sold = t.Sold?.ToDateTimeOffset(),
                    SoldPrice = t.SoldPrice != null ?
                        new GrossPriceViewModel
                        {
                            // Logic for simplify price
                            GrossPrice = t.Product.Price?.GrossPrice > t.SoldPrice.GrossPrice ?
                                        (decimal)t.Product.Price?.GrossPrice :
                                        (decimal)t.SoldPrice.GrossPrice,
                            TaxPercentage = t.SoldPrice.TaxPercentage
                        } :
                        null,
                    ProductId = t.Product.Id,
                    Name = t.Product.Name,
                };
                switch (t.Product.ProductType)
                {
                    case ProductType.ColdDrink:
                        productItem.ProductType = "Cold Drink";
                        productItem.Litre = (decimal)t.Product.Litre;
                        productItem.TemperatureMaximum = (decimal)t.Product.TemperatureMaximum;
                        productItem.TemperatureMinimum = (decimal)t.Product.TemperatureMinimum;
                        break;
                    case ProductType.HotDrink:
                        productItem.ProductType = "Hot Drink";
                        productItem.Grams = (decimal)t.Product.Grams;
                        productItem.TemperatureMaximum = (decimal)t.Product.TemperatureMaximum;
                        productItem.TemperatureMinimum = (decimal)t.Product.TemperatureMinimum;
                        break;
                    case ProductType.Snack:
                        productItem.ProductType = "Snack";
                        productItem.Grams = (decimal)t.Product.Grams;
                        break;
                }
                productsVM.Add(productItem);
            });

            return new MachineItemViewModels
            {
                Id = machine.MachineId,
                MachineType = new MachineTypeViewModel { 
                    ModelName = machine.MachineType.Model,
                    Version = (short)machine.MachineType.Version
                },
                Position = new MapPointViewModel
                {
                    X = (decimal)machine.Position.X,
                    Y = (decimal)machine.Position.Y
                },
                Status = machine.Status,
                Temperature = (decimal?)machine.Temperature,
                CoinsCurrentSupply = (decimal?)machine.CoinsCurrentSupply,
                CoinsInMachines = (decimal?)machine.CoinsInMachine,
                MoneyFromBirth = (decimal)machine.MoneyFromBirth,
                MoneyMonth = (decimal)machine.MoneyMonth,
                MoneyYear = (decimal)machine.MoneyYear,
                LatestCleaningMachine = machine.LatestCleaningMachine?.ToDateTimeOffset(),
                LatestLoadedProducts = machine.LatestLoadedProducts?.ToDateTimeOffset(),
                LatestMoneyCollection = machine.LatestMoneyCollection?.ToDateTimeOffset(),
                ProductItem = productsVM
            };
        }
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
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string Name { get; set; }
        public decimal TemperatureMinimum { get; set; }
        public decimal TemperatureMaximum { get; set; }
        public decimal Litre { get; set; }
        public decimal Grams { get; set; }
        public string ProductType { get; set; }
        public GrossPriceViewModel SoldPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTimeOffset? Purchased { get; set; }
        public DateTimeOffset? Sold { get; set; }
    }

    public class GrossPriceViewModel
    {
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }
}
