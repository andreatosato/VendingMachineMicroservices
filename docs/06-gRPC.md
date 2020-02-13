# gRPC
Nuovo standard creato da Google che da come beneficio una maggior velocità di scambio dei dati, dato anche dal canale (gira su HTTP/2) che consente una connessione permanente.
Possibilità di inviare dati in streaming.
Il Client e il server vengono generati da classi proxy

### Protobuffer
Libreria che consente di comprimere il payload prima di spedirlo sul canale.

```protobuf
syntax = "proto3";

import "Protos/machine-models.proto";

option csharp_namespace = "VendingMachine.Service.Machines.ServiceCommunications";

package VendingMachine.Service.Machines.ServiceCommunications;

service MachineItems {
	rpc ExistMachine (ExistMachineRequest) returns (ExistMachineResponse);
	rpc GetMachineInfos (GetMachineInfoRequest) returns (GetMachineInfoResponse);
	rpc GetMachineStatus (MachineStatusRequest) returns (MachineStatusResponse);
}
```
Oltre a ...

```protobuf
syntax = "proto3";

/*Namespace per i DateTime C#*/
import "google/protobuf/timestamp.proto";
/*Namespace per i tipi nullable ESPLICITI (double?) => google.protobuf.DoubleValue C#*/
import "google/protobuf/wrappers.proto";

/*Models*/
message MachineServiceModel {
	MapPosition Position = 1;
	google.protobuf.DoubleValue Temperature = 2;
	google.protobuf.BoolValue Status = 3;
	google.protobuf.DoubleValue MoneyFromBirth = 4;
    google.protobuf.DoubleValue MoneyMonth = 5;
    google.protobuf.DoubleValue MoneyYear = 6;
    repeated ProductActiveModel ActiveProducts = 8;
    google.protobuf.Timestamp LatestLoadedProducts = 9;
    google.protobuf.Timestamp LatestCleaningMachine = 10;
    google.protobuf.Timestamp LatestMoneyCollection = 11;
    google.protobuf.DoubleValue CoinsInMachine = 12;
    google.protobuf.DoubleValue CoinsCurrentSupply = 13;
	int32 machineId = 14;
	MachineTypeModel MachineType = 15;
}

message ProductConsumedModel {
	int32 Id = 1;
	google.protobuf.Timestamp ActivationDate = 2;
	google.protobuf.Timestamp ProvidedDate = 3;
}

message ProductActiveModel {
	int32 Id = 1;
	google.protobuf.Timestamp ActivationDate = 2;
}

message MapPosition {
	double X = 1;
	double Y = 2;
}

message MachineTypeModel {
	string Model = 1;
	MachineVersion Version = 2;
}

enum MachineVersion {
     Coffee = 0;
     Frigo = 1;
     FrigoAndCoffee = 2;
}



/*Request and Response*/
message GetMachineInfoRequest {
	int32 MachineId = 1;
}

message GetMachineInfoResponse {
	MachineServiceModel Machine = 1;
}

message ExistMachineRequest {
	int32 MachineId = 1;
}

message ExistMachineResponse {
	bool Exist = 1;
}

message MachineStatusRequest {
	int32 MachineId = 1;
}

message MachineStatusResponse {
	int32 MachineId = 1;
	double CoinCurrentSupply = 2;
}
```
Server

```csharp
public class MachineItemService : MachineItems.MachineItemsBase
{
    private readonly IMachineQuery machineQuery;

    public MachineItemService(IMachineQuery machineQuery)
    {
        this.machineQuery = machineQuery;
    }

    public override async Task<GetMachineInfoResponse> GetMachineInfos(GetMachineInfoRequest request, ServerCallContext context)
    {
        if (request.MachineId > 0)
        {
            var result = await machineQuery.GetMachineItemInfoAsync(request.MachineId).ConfigureAwait(false);
            GetMachineInfoResponse response = new GetMachineInfoResponse().ToResponse(result);
            return response;
        }

        throw new ArgumentNullException();
    }

    public override async Task<ExistMachineResponse> ExistMachine(ExistMachineRequest request, ServerCallContext context)
    {
        var exist = await machineQuery.CheckMachineItemExistsAsync(request.MachineId).ConfigureAwait(false);
        return new ExistMachineResponse { Exist = exist };
    }

    public override async Task<MachineStatusResponse> GetMachineStatus(MachineStatusRequest request, ServerCallContext context)
    {
        if (request.MachineId > 0)
        {
            var result = await machineQuery.GetMachineItemStatusAsync(request.MachineId).ConfigureAwait(false);
            MachineStatusResponse response = new MachineStatusResponse() {
                CoinCurrentSupply = (double) result.CoinsCurrentSupply, 
                MachineId = result.MachineItemId
            };
            return response;
        }
        throw new ArgumentNullException();
    }
}
```

```csharp

```