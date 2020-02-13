# Startup Progetto
Prima di partire con il workshop è necessario che la soluzione vuota compili.

# Pre-Requisiti
- dotnet ef tool
	- Per installarlo: **dotnet tool install --global dotnet-ef**

1. Scaricare il progetto
2. Compilare tutta soluzione

# Creazione dei database
La creazione dei DB ci serve per creare le tabelle e inserire i dati di partenza.
Aprire powershell oppure dalla console di Visual Studio:

1. **dotnet ef database update --project "VendingMachine.Service.Authentications.API"**
2. **dotnet ef database update --project "VendingMachine.Service.Machines.Data"**
3. **dotnet ef database update --project "VendingMachine.Service.Products.Data"**
4. **dotnet ef database update --project "VendingMachine.Service.Orders.Data"**

Alcuni dati verrano importati da dei [Job esterni](/08-Worker.md).
