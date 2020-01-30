using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Workers.ImporterProducts.Readers.Product
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
            var layout = new ProductLayout();
            var factory = new FlatFile.Delimited.Implementation.DelimitedFileEngineFactory();
            using (var stream = new MemoryStream(await File.ReadAllBytesAsync(FullName)))
            {
                var flatFile = factory.GetEngine(layout);
                var records = flatFile.Read<Product>(stream).ToArray();
                // TODO: create product
            }
        }
    }
}
