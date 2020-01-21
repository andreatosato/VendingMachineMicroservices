using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.Models;
using VendingMachine.Service.Products.Read.Queries;

namespace VendingMachine.Service.Products.ServiceCommunications.Services
{
    // https://docs.microsoft.com/it-it/dotnet/architecture/grpc-for-wcf-developers/protobuf-data-types
    public class ProductItemsService : ProductItems.ProductItemsBase
    {
        private readonly IProductItemQuery query;
        public ProductItemsService(IProductItemQuery query)
        {
           this.query = query;
        }
        public override async Task GetProducts(ProductsRequest request, IServerStreamWriter<ProductItemsServiceModel> responseStream, ServerCallContext context)
        {
            ProductItemsServiceModel response = new ProductItemsServiceModel();
            List<int> productIds = new List<int>();
            while (request.ProductIds.GetEnumerator().MoveNext())
            {
                productIds.Add(request.ProductIds.GetEnumerator().Current);
            }
            Read.Models.ProductItemsReadModel productItems = await query.GetProductsInfoAsync(productIds);
            foreach (var pi in productItems.Products)
            {
                var productItem = new ProductItemsServiceModel
                {
                    ExpirationDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(pi.ExpirationDate),
                    Purchased = pi.Purchased.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(pi.Purchased.GetValueOrDefault()) : null,
                    Sold = pi.Sold.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(pi.Sold.GetValueOrDefault()) : null,
                    SoldPrice = new GrossPriceServiceModel
                    {
                        GrossPrice = (double)pi.SoldPrice.GrossPrice,
                        TaxPercentage = pi.SoldPrice.TaxPercentage
                    },
                    Product = new ProductServiceModel
                    {
                        Id = pi.Product.Id,
                        Name = pi.Product.Name,
                        Price = new GrossPriceServiceModel { 
                            GrossPrice = (double) pi.Product.Price.GrossPrice,
                            TaxPercentage = pi.Product.Price.TaxPercentage
                        }                        
                    }
                };
                if (pi.Product is ColdDrinkReadModel coldDrink)
                {
                    productItem.Product.TemperatureMinimum = (double)coldDrink.TemperatureMinimum;
                    productItem.Product.TemperatureMaximum = (double)coldDrink.TemperatureMaximum;
                    productItem.Product.ProductType = ProductType.ColdDrink;
                }
                else if(pi.Product is HotDrinkReadModel hotDrink)
                {
                    productItem.Product.TemperatureMinimum = (double)hotDrink.TemperatureMinimum;
                    productItem.Product.TemperatureMaximum = (double)hotDrink.TemperatureMaximum;
                    productItem.Product.Grams = (double)hotDrink.Grams;
                    productItem.Product.ProductType = ProductType.HotDrink;
                }
                else if(pi.Product is SnackReadModel snack)
                {
                    productItem.Product.Grams = (double)snack.Grams;
                    productItem.Product.ProductType = ProductType.Snack;
                }

                await responseStream.WriteAsync(productItem).ConfigureAwait(false);
            }
        }

    }
}
