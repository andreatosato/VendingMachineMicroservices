﻿using Microsoft.SqlServer.Types;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Read.Models;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read
{
    public interface IMachineQuery : IQuery
    {
        public Task<IEnumerable<NearbyMachineReadModel>> GetNearbyMachinesAsync(SqlGeography currentPosition, decimal radius);
        public Task<CoinsInMachineReadModel> GetCoinsInMachineAsync(int machineId);
    }
}