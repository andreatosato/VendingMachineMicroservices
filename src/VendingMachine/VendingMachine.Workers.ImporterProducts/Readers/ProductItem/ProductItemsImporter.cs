using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var layout = new ProductItemLayout();
            var factory = new FlatFile.Delimited.Implementation.DelimitedFileEngineFactory();
            using (var stream = new MemoryStream(await File.ReadAllBytesAsync(FullName)))
            {
                var flatFile = factory.GetEngine(layout);
                var records = flatFile.Read<ProductItem>(stream).ToArray();
                // TODO: create product Item
            }
        }
    }
}
