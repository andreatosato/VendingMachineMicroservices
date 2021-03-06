# VendingMachineMicroservices
Vending machine microservices

# Pre-Requisiti
- dotnet ef tool
	- Per installarlo: dotnet tool install --global dotnet-ef

# Entity Framework Migration
Multitarget projects:  <TargetFrameworks>netcoreapp3.1;netstandard2.1</TargetFrameworks>
- Install: Microsoft.EntityFrameworkCore.Design
- dotnet ef migrations add FirstMigrationMachines --project "VendingMachine.Service.Machines.Data"
- dotnet ef database update --project "VendingMachine.Service.Machines.Data"

Per rimuovere le migrazioni:
- dotnet ef migrations remove --project "VendingMachine.Service.Machines.Data"

A seguire:
- dotnet ef migrations add SecondMigrationMachines --project "VendingMachine.Service.Machines.Data"



dotnet ef migrations add SecondMigrationAuth --project "VendingMachine.Service.Authentications.API"
 dotnet ef database update --project "VendingMachine.Service.Authentications.API"


#EF Migration Seed Limitation
- https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding#limitations-of-model-seed-data
This type of seed data is managed by migrations and the script to update the data that's already in the database needs to be generated without connecting to the database. This imposes some restrictions:

The primary key value needs to be specified even if it's usually generated by the database. It will be used to detect data changes between migrations.
Previously seeded data will be removed if the primary key is changed in any way.



#Funzionalit� applicative
- vending machine 
	- machine type: macchina, tipo, modello
	- machine item: (soldi raccolti dall'installazione, soldi raccolti nel mese, soldi raccolti nell'anno, 
							 lista prodotti inseriti nel tempo, lista prodotti ancora attivi, posizione gps,
							 ultima data caricamento prodotti, ultima data pulizia, ultima data raccolta monete)
	Machine Type: descrive il singolo modello della macchina.
	Machine item � la macchina installata presso il cliente
- products
	- product: [prodotto base] (caratteristiche, marca, tipo di erogazione, prezzo, temperature di conservazione)
	- product items: [prodotto a listino] (data acquisto, data scadenza, *vending machine item*).
	Products descrive il prodotto
	Product item descrive il singolo prodotto nella vending machine.
- orders
	- order: (product items list, soldi inseriti, resto, vending machine item, data acquisto)

Quindi in questo secondo servizio andrei ad estrarre i prodotti presenti nella singola macchina. Per ogni macchina potrei estrarre lo stato attuale con il numero di prodotti presenti.
Inoltre i prodotti venduti, per giorno, per mese etc.



#Log
Serilog: https://github.com/serilog/serilog-aspnetcore


#Mediatr
https://github.com/jbogard/MediatR/wiki


#HealthCheck
https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks


#API Ports
| Machine.API | 4010 |
| Product.API | 4020 |
| Order.API | 4030 |
| Aggregator.API | 4040 | 


#Step 1 - Align data environment
Create database and update data
1. dotnet ef database update --project "VendingMachine.Service.Authentications.API"
2. dotnet ef database update --project "VendingMachine.Service.Machines.Data"
3. dotnet ef database update --project "VendingMachine.Service.Products.Data"
4. dotnet ef database update --project "VendingMachine.Service.Orders.Data"
5. Run All app projects
6. Copy VendingMachine.Workers.ImporterProducts\FileImport to %TEMP%\Workshop\VendingMachine
... Products.csv
... ProductItems.csv



#Utenti
| Username | Password |
|----------|----------|
| test@cloudgen.it | Pass123$ |
| andrea.tosato@4ward.it | Pass123$ |