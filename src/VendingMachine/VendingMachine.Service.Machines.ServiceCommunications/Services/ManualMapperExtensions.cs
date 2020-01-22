using System.Linq;
using VendingMachine.Service.Machines.Read.Models;

namespace VendingMachine.Service.Machines.ServiceCommunications.Services
{
    public static class ManualMapperExtensions
    {
        public static GetMachineInfoResponse ToResponse(this GetMachineInfoResponse response, MachineItemReadModel readModel)
        {
            response.Machine.MachineId = readModel.Id;
            response.Machine.CoinsCurrentSupply = (double)readModel.CoinsCurrentSupply;
            response.Machine.CoinsInMachine = (double)readModel.CoinsInMachine;
            response.Machine.LatestCleaningMachine = readModel.LatestCleaningMachine.HasValue ?
                Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestCleaningMachine.Value) :
                null;
            response.Machine.LatestLoadedProducts = readModel.LatestCleaningMachine.HasValue ?
                Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestLoadedProducts.Value) :
                null;
            response.Machine.LatestMoneyCollection = readModel.LatestCleaningMachine.HasValue ?
                Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(readModel.LatestMoneyCollection.Value) :
                null;
            response.Machine.MoneyFromBirth = (double)readModel.MoneyFromBirth;
            response.Machine.MoneyMonth = (double)readModel.MoneyMonth;
            response.Machine.MoneyYear = (double)readModel.MoneyYear;
            response.Machine.Status = readModel.Status;
            response.Machine.Temperature = (double)readModel.Temperature;
            response.Machine.Position = new MapPosition()
            {
                X = (double)readModel.Position.X,
                Y = (double)readModel.Position.Y,
            };
            response.Machine.MachineType = new MachineTypeModel
            {
                Model = readModel.MachineType.Model,
                Version = (MachineVersion)readModel.MachineType.Version
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
