using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Service.Gateway.RefitModels;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public class ProductImporter : IProductImporter
    {
        private readonly IServiceProvider serviceProvider;

        public ProductImporter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DoWorkAsync(string Name, string FullName)
        {
            IGatewayApi api = (IGatewayApi)serviceProvider.GetService(typeof(IGatewayApi));
            var layout = new ProductLayout();
            var factory = new FlatFile.Delimited.Implementation.DelimitedFileEngineFactory();
            using (var stream = new MemoryStream(await File.ReadAllBytesAsync(FullName)))
            {
                var flatFile = factory.GetEngine(layout);
                var records = flatFile.Read<Product>(stream).ToArray();

                foreach (var product in records)
                {
                    switch (product.ProductType)
                    {
                        case "ColdDrink":
                            var coldDrink = new Service.Products.Application.ViewModels.Products.ColdDrinkViewModel
                            { 
                                Name = product.Name,
                                Price = new Service.Products.Application.ViewModels.Products.GrossPriceViewModel
                                {
                                    GrossPrice = product.GrossPrice,
                                    TaxPercentage = product.TaxPercentage
                                },
                                TemperatureMinimum = product.TemperatureMinimum,
                                TemperatureMaximum = product.TemperatureMaximum,
                                Litre = product.Litre
                            };
                            await api.PostCreateColdDrinkAsync(coldDrink);
                            break;
                        case "HotDrink":
                            var hotDrink = new Service.Products.Application.ViewModels.Products.HotDrinkViewModel
                            {
                                Name = product.Name,
                                Price = new Service.Products.Application.ViewModels.Products.GrossPriceViewModel
                                {
                                    GrossPrice = product.GrossPrice,
                                    TaxPercentage = product.TaxPercentage
                                },
                                TemperatureMinimum = product.TemperatureMinimum,
                                TemperatureMaximum = product.TemperatureMaximum,
                                Grams = product.Grams
                            };
                            await api.PostCreateHotDrinkAsync(hotDrink);
                            break;
                        case "Snack":
                            var snack = new Service.Products.Application.ViewModels.Products.SnackViewModel
                            {
                                Name = product.Name,
                                Price = new Service.Products.Application.ViewModels.Products.GrossPriceViewModel
                                {
                                    GrossPrice = product.GrossPrice,
                                    TaxPercentage = product.TaxPercentage
                                },
                                Grams = product.Grams
                            };
                            await api.PostCreateSnackAsync(snack);
                            break;
                        default:
                            //LOG
                            break;
                    }
                }

            }
        }
    }
}
