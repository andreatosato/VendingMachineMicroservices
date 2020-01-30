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


        public override async Task GetProductItems(GetProductItemsRequest request, IServerStreamWriter<ProductItemsServiceModel> responseStream, ServerCallContext context)
        {
            List<int> productIds = new List<int>();
            using (IEnumerator<int> productIdEnum = request.ProductIds.GetEnumerator())
            {
                while (productIdEnum.MoveNext())
                {
                    productIds.Add(productIdEnum.Current);
                }
            }
            Read.Models.ProductItemsReadModel productItems = await query.GetProductItemsInfoAsync(productIds);
            foreach (var pi in productItems.Products)
            {
                var productItem = new ProductItemsServiceModel
                {
                    Id = pi.Id,
                    ExpirationDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(pi.ExpirationDate),
                    Purchased = pi.Purchased.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(pi.Purchased.GetValueOrDefault()) : null,
                    Sold = pi.Sold.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(pi.Sold.GetValueOrDefault()) : null,
                    SoldPrice = pi.SoldPrice != null ? 
                        new GrossPriceServiceModel { GrossPrice = (double)pi.SoldPrice.GrossPrice, TaxPercentage = pi.SoldPrice.TaxPercentage } :
                        null
                    ,
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

        public override async Task<ExistProductItemResponse> ExistProductItem(ExistProductItemRequest request, ServerCallContext context)
        {
            bool result = await query.ExistProductItemAsync(request.ProductItemId);
            return new ExistProductItemResponse() { Exist = result };
        }
    }
}
