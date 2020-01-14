using Dapper;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read.Models;

namespace VendingMachine.Service.Machines.Read
{
    public class MachinesQuery : IMachineQuery
    {
        private readonly string machineConnectionString;

        public MachinesQuery(string machineConnectionString)
        {
            this.machineConnectionString = machineConnectionString;
        }

        public async Task<ProductsReadModel> GetProductsInMachineAsync(int machineId)
        {
            ProductsReadModel result = new ProductsReadModel();
            using (SqlConnection connection = new SqlConnection(machineConnectionString))
            {
                var article = await connection
                    .QueryAsync<ProductReadModel>(@"SELECT Id
                        FROM dbo.ActiveProducts
                        Where MachineItemId = @Id", new { Id = machineId })
                    .ConfigureAwait(false);
                result.Products.AddRange(article);
            }
            return result;
        }

        public async Task<bool> CheckActiveProductExist(int productId)
        {
            using (SqlConnection connection = new SqlConnection(machineConnectionString))
            {
                var p = await connection
                    .QueryAsync<ProductReadModel>(@"SELECT Id
                        FROM dbo.ActiveProducts
                        Where Id = @Id", new { Id = productId })
                    .ConfigureAwait(false);
                return p != null;
            }
        }

        public async Task<bool> CheckHistoryProductExist(int productId)
        {
            using (SqlConnection connection = new SqlConnection(machineConnectionString))
            {
                var p = await connection
                    .QueryAsync<ProductReadModel>(@"SELECT Id
                        FROM dbo.HistoryProducts
                        Where Id = @Id", new { Id = productId })
                    .ConfigureAwait(false);
                return p.Any();
            }
        }

        public async Task<HistoryProductsReadModel> GetHistoryProductsInMachineAsync(int machineId)
        {
            HistoryProductsReadModel result = new HistoryProductsReadModel();
            using (SqlConnection connection = new SqlConnection(machineConnectionString))
            {
                var article = await connection
                    .QueryAsync<HistoryProductReadModel>(@"SELECT Id, ProvidedDate, ActivationDate 
                        FROM dbo.HistoryProducts
                        Where MachineItemId = @Id", new { Id = machineId })
                    .ConfigureAwait(false);
                result.Products.AddRange(article);
            }
            return result;
        }

        public async Task<CoinsInMachineReadModel> GetCoinsInMachineAsync(int machineId)
        {
            CoinsInMachineReadModel result = null;
            using (SqlConnection connection = new SqlConnection(machineConnectionString))
            {
                result  = await connection
                    .QueryFirstAsync<CoinsInMachineReadModel>(@"SELECT CoinsInMachine, CoinsCurrentSupply 
                        FROM dbo.Machines
                        WHERE Id = @Id", new { Id = machineId })
                    .ConfigureAwait(false);
            }
            return result;
        }

        public async Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachinesAsync(SqlGeography currentPosition, decimal radius)
        {
            throw new NotImplementedException();
        }

        public async Task<MachineItemReadModel> GetMachineItemInfoAsync(int machineId)
        {
            MachineItemReadModel mapping = null;
            MachineItemReadModel result = null;
            using (System.Data.IDbConnection connection = new SqlConnection(machineConnectionString))
            {
                string sql = @"SELECT M.[Id]
                    ,[Position]
                    ,[Temperature]
                    ,[Status]
                    ,[MachineTypeId]
                    ,[LatestLoadedProducts]
                    ,[LatestCleaningMachine]
                    ,[LatestMoneyCollection]
                    ,[CoinsInMachine]
                    ,[CoinsCurrentSupply],
                    P.Id,
                    P.ActivationDate
                FROM [VendingMachine-Machines].[dbo].[Machines] AS M
                LEFT JOIN [VendingMachine-Machines].[dbo].[ActiveProducts] AS P ON M.Id = P.MachineItemId
                WHERE M.Id = @Id";
                
                result = (await connection
                    .QueryAsync<MachineItemDapper, ActiveProductReadModel, MachineItemReadModel>(
                        sql: sql, 
                        map: (m, p) => 
                        {
                            if (mapping == null)
                            {
                                mapping = new MachineItemReadModel();
                                mapping.FromDapper(m, new MapPointReadModel(m.Position.Lat.Value, m.Position.Long.Value));
                            }
                            mapping.ActiveProducts.Add(p);
                            return mapping;
                        },
                        splitOn: "Id",
                        param: new { Id = machineId })
                    .ConfigureAwait(false)).FirstOrDefault();
            }
            return result;
        }

        public async Task<bool> CheckMachineItemExistsAsync(int machineId)
        {
            using (System.Data.IDbConnection connection = new SqlConnection(machineConnectionString))
            {
                string sql = @"SELECT 1 AS Result
                FROM [VendingMachine-Machines].[dbo].[Machines] AS M
                WHERE M.Id = @Id";

                // Install Microsoft.CSharp for use Dynamic data
                var exists = await connection
                    .QueryAsync(
                        sql: sql,
                        param: new { Id = machineId })
                    .ConfigureAwait(false);

                if(exists.FirstOrDefault().Result == 1)
                    return true;
                return false;
            }
        }
    }
}
