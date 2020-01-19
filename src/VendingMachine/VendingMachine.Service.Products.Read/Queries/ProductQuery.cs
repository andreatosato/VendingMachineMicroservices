using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.DapperModels;
using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Products.Read.Queries
{
    public class ProductQuery : IProductQuery
    {
        private readonly string productConnectionString;

        public ProductQuery(string productConnectionString)
        {
            this.productConnectionString = productConnectionString;
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
    }
}
