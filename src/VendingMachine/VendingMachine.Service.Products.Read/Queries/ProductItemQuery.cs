using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.DapperModels;
using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Products.Read.Queries
{
    public class ProductItemQuery : IProductItemQuery
    {
        private readonly string productConnectionString;

        public ProductItemQuery(string productConnectionString)
        {
            this.productConnectionString = productConnectionString;
        }

        public async Task<bool> ExistProductItemAsync(int productItemsId)
        {
            bool result;
            using SqlConnection connection = new SqlConnection(productConnectionString);
            result = await connection
                .ExecuteScalarAsync<bool>(@"SELECT COUNT(DISTINCT 1) FROM [dbo].[ProductItems] WHERE Id = @Id", 
                    param: new { Id = productItemsId })
                .ConfigureAwait(false);
            return result;
        }

        public async Task<ProductItemReadModel> GetProductInfoAsync(int productItemId)
        {
            ProductItemReadModel result = new ProductItemReadModel();

            using SqlConnection connection = new SqlConnection(productConnectionString);
            var product = await connection
                .QueryAsync<ProductItemReadModel, ProductFullDapperModel, ProductItemReadModel>(
                    @"SELECT PI.Id, PI.ExpirationDate, PI.Sold, PI.Purchased, PI.GrossPrice, PI.NetPrice, PI.TaxPercentage, PI.Rate,
                    P.[Id],P.[GrossPrice],P.[NetPrice],P.[TaxPercentage],P.[Rate],[Name],[Version],[Discriminator],[TemperatureMinimum],[TemperatureMaximum],[Litre],[Grams]
                    FROM [dbo].[ProductItems] PI
                    INNER JOIN [dbo].[Products] P ON PI.ProductId = P.Id
                    WHERE PI.Id = @Id",
                    map: (item, p) =>
                    {
                        item.Product = p.ToReadModel();
                        item.SoldPrice = new GrossPriceReadModel
                        {
                            GrossPrice = p.GrossPrice,
                            NetPrice = p.NetPrice,
                            Rate = p.Rate,
                            TaxPercentage = p.TaxPercentage
                        };
                        return item;
                    },
                    param: new { Id = productItemId })
                .ConfigureAwait(false);

            if (product.AsList().Any())
                result = product.FirstOrDefault();

            return result;
        }

        public async Task<ProductItemsReadModel> GetProductItemsInfoAsync(List<int> productItemsId)
        {
            ProductItemsReadModel result = new ProductItemsReadModel();
            using SqlConnection connection = new SqlConnection(productConnectionString);
            result.Products = await connection
                .QueryAsync<ProductItemReadModel, ProductFullDapperModel, ProductItemReadModel>(
                    @"SELECT PI.Id, PI.ExpirationDate, PI.Sold, PI.Purchased, PI.GrossPrice, PI.NetPrice, PI.TaxPercentage, PI.Rate,
                    P.[Id],P.[GrossPrice],P.[NetPrice],P.[TaxPercentage],P.[Rate],[Name],[Version],[Discriminator],[TemperatureMinimum],[TemperatureMaximum],[Litre],[Grams]
                    FROM [dbo].[ProductItems] PI
                    INNER JOIN [dbo].[Products] P ON PI.ProductId = P.Id
                    WHERE PI.Id IN @Ids",
                    map: (item, p) => 
                    {
                        item.Product = p.ToReadModel();
                        item.SoldPrice = new GrossPriceReadModel
                        {
                            GrossPrice = p.GrossPrice,
                            NetPrice = p.NetPrice,
                            Rate = p.Rate,
                            TaxPercentage = p.TaxPercentage
                        };
                        return item;
                    },
                    param: new { Ids = productItemsId.ToArray() })
                .ConfigureAwait(false);
            
            return result;
        }
    }
}
