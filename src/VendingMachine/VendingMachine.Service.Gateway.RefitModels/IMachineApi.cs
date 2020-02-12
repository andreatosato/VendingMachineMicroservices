using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Application.ViewModels;
using VendingMachine.Service.Machines.Read.Models;

namespace VendingMachine.Service.Gateway.RefitModels
{
    [Headers("Authorization: Bearer")]
    public interface IMachineApi
    {
        [Get("/machine-api/MachineItems/NearbyMachineItems")]
        Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachineItemsAsync([Body] GeoSearchViewModel model);
    }
}
