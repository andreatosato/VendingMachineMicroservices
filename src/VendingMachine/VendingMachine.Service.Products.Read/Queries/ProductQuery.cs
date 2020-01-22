using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.DapperModels;
using VendingMachine.Service.Products.Read.Models;
using System.Collections.ObjectModel;

namespace VendingMachine.Service.Products.Read.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly string productConnectionString;

        public ProductQuery(string productConnectionString)
        {
            this.productConnectionString = productConnectionString;
        }

        public async Task<bool> ExistProductAsync(int productId)
        {
            bool result;
            using SqlConnection connection = new SqlConnection(productConnectionString);
            result = await connection
                .ExecuteScalarAsync<bool>(@"SELECT COUNT(DISTINCT 1) FROM [dbo].[Products] WHERE PI.Id IN @Id",
                    param: new { Id = productId })
                .ConfigureAwait(false);
            return result;
        }

        public async Task<ProductReadModel> GetProductInfoAsync(int productId)
        {
            using SqlConnection connection = new SqlConnection(productConnectionString);
            var product = await connection
                .QueryFirstAsync<ProductFullDapperModel>(
                    @"SELECT [Id],[GrossPrice],[NetPrice],[TaxPercentage],[Rate],[Name],[Version],[Discriminator],[TemperatureMinimum],[TemperatureMaximum],[Litre],[Grams]
                    FROM dbo.Products
                    Where Id = @Id", new { Id = productId })
                .ConfigureAwait(false);
            return product.ToReadModel();
        }

        public async Task<ProductsReadModel> GetProductsInfoAsync(List<int> productId)
        {
            ProductsReadModel result = new ProductsReadModel();
            using SqlConnection connection = new SqlConnection(productConnectionString);
            var products = await connection
                .QueryAsync<ProductFullDapperModel>(
                    @"SELECT [Id],[GrossPrice],[NetPrice],[TaxPercentage],[Rate],[Name],[Version],[Discriminator],[TemperatureMinimum],[TemperatureMaximum],[Litre],[Grams]
                    FROM dbo.Products
                    Where Id IN @Ids", new { Ids = productId.ToArray() })
                .ConfigureAwait(false);
            foreach (var item in products)
            {
                result.Products.Add(item.ToReadModel());
            }
            return result;
        }
    }
}
