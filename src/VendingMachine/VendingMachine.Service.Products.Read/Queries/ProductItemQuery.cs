﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public async Task<ProductItemsReadModel> GetProductsInfoAsync(List<int> productItemsId)
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
                        return item;
                    },
                    param: new { Ids = productItemsId.ToArray() })
                .ConfigureAwait(false);
            
            return result;
        }
    }
}