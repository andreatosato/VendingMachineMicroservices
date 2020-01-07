using Dapper;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read.Models;
using VendingMachine.Service.Shared.Read;

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
                    .QueryFirstAsync<ProductReadModel>(@"SELECT Id
                        FROM dbo.ActiveProducts
                        Where MachineItemId = @Id", new { Id = machineId })
                    .ConfigureAwait(false);
                result.Products.Add(article);
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
    }
}
