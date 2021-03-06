﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Products.Read.Models;

namespace VendingMachine.Service.Products.Read.Queries
{
    public interface IProductItemQuery
    {
        Task<ProductItemsReadModel> GetProductItemsInfoAsync(List<int> productItemsId);
        Task<ProductItemReadModel> GetProductInfoAsync(int productItemId);
        Task<bool> ExistProductItemAsync(int productItemsId);
    }
}
