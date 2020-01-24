using System;
using System.Linq;
using VendingMachine.Service.Machines.Read.Models;

namespace VendingMachine.Service.Machines.ServiceCommunications.Services
{
    public static class ManualMapperExtensions
    {
        public static GetMachineInfoResponse ToResponse(this GetMachineInfoResponse response, MachineItemReadModel readModel)
        {
            response.Machine = new MachineServiceModel
            {
                MachineId = readModel.Id,
                CoinsCurrentSupply = (double)readModel.CoinsCurrentSupply,
                CoinsInMachine = (double)readModel.CoinsInMachine,
                LatestCleaningMachine = readModel.LatestCleaningMachine.HasValue ?
                    Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestCleaningMachine.Value) :
                    null,
                LatestLoadedProducts = readModel.LatestLoadedProducts.HasValue ?
                    Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestLoadedProducts.Value) :
                    null,
                LatestMoneyCollection = readModel.LatestMoneyCollection.HasValue ?
                    Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestMoneyCollection.Value) :
                    null,
                MoneyFromBirth = (double)readModel.MoneyFromBirth,
                MoneyMonth = (double)readModel.MoneyMonth,
                MoneyYear = (double)readModel.MoneyYear,
                Status = readModel.Status,
                Temperature = (double)readModel.Temperature,
                Position = new MapPosition()
                {
                    X = (double)readModel.Position.X,
                    Y = (double)readModel.Position.Y,
                },
                MachineType = new MachineTypeModel
                {
                    Model = readModel.MachineType.Model,
                    Version = Enum.Parse<MachineVersion>(readModel.MachineType.Version)
                }
            };
            var activeProducts = readModel.ActiveProducts.Select(x => new ProductActiveModel
            {
                Id = x.Id,
                ActivationDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(x.ActivationDate)
            });
            response.Machine.ActiveProducts.Add(activeProducts);

            return response;
        }
    }
}
