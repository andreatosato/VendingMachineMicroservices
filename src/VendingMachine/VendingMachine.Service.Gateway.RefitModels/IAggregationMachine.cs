using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Aggregators.Web.API.ViewModels.Machine;

namespace VendingMachine.Service.Gateway.RefitModels
{
    [Headers("Authorization: Bearer")]
    public interface IAggregationMachine
    {
        [Get("/aggregator-api/AggregatorMachine/{machineId}")]
        Task<MachineItemViewModels> GetMachineCurrentStatusAsync(int machineId);
    }
}
