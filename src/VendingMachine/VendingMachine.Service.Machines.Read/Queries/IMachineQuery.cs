using Microsoft.SqlServer.Types;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read.Models;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read
{
    public interface IMachineQuery : IQuery
    {
        Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachinesAsync(SqlGeography currentPosition, decimal radius);
        Task<CoinsInMachineReadModel> GetCoinsInMachineAsync(int machineId);
        Task<ProductsReadModel> GetProductsInMachineAsync(int machineId);
        Task<HistoryProductsReadModel> GetHistoryProductsInMachineAsync(int machineId);
        Task<MachineItemReadModel> GetMachineItemInfoAsync(int machineId);
        Task<bool> CheckMachineItemExistsAsync(int machineId);
        Task<bool> CheckActiveProductExist(int productId);
        Task<bool> CheckHistoryProductExist(int productId);
    }
}
