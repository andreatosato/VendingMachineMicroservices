using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Service.Gateway.RefitModels;

namespace VendingMachine.Workers.ImporterProducts.Readers
{
    public class ProductItemsImporter : IProductItemsImporter
    {
        private readonly IServiceProvider serviceProvider;

        public ProductItemsImporter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DoWorkAsync(string Name, string FullName)
        {
            try 
            {
                IGatewayApi api = (IGatewayApi)serviceProvider.GetService(typeof(IGatewayApi));
                var layout = new ProductItemLayout();
                var factory = new FlatFile.Delimited.Implementation.DelimitedFileEngineFactory();
                using (var stream = new MemoryStream(await File.ReadAllBytesAsync(FullName)))
                {
                    var flatFile = factory.GetEngine(layout);
                    var records = flatFile.Read<ProductItem>(stream).ToArray();

                    foreach (var product in records)
                    {
                        var productItem = new Service.Products.Application.ViewModels.ProductItems.ProductItemViewModel
                        {
                            ExpirationDate = product.ExpirationDate,
                            ProductId = product.ProductId,
                            Purchased = product.Purchased,
                            SoldPrice = product.RedefinedPrice
                        };
                        await api.PostCreateProductItemAsync(productItem);
                    }
                }

            }
            catch (Exception ex)
            {
                // TODO: Log

                throw;
            }
}
    }
}
