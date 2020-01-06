using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data.Seeds
{
    public static partial class MigrationExtension
    {
        public static MigrationBuilder _1(this MigrationBuilder builder)
        {
            builder.InsertData("MachineTypes",
                new[] {"Model", "Version" },
                new object [] { "BVM", "Coffee" });

            builder.InsertData("MachineTypes",
                new[] {"Model", "Version" },
                new object[] {"MiniSnakky", "FrigoAndCoffee" });

            builder.InsertData("Machines",
                new[] {"DataCreated", "DataUpdated", "Position", "Temperature", "Status", "MachineTypeId" },
                new object[] { DateTime.UtcNow, null, SeedBase.SetPosition(45.4351m, 10.9988m), "2", true, 2 });

            return builder;
        }

        // Choose manual migrations
        //public static ModelBuilder SeedData(this ModelBuilder modelBuilder)
        //{
        //    var coffeeMachineBVM = new MachineType("BVM", MachineType.MachineVersion.Coffee) { Id = 1 };
        //    var frigoAndCoffeeMiniSnakky = new MachineType("MiniSnakky", MachineType.MachineVersion.FrigoAndCoffee) { Id = 2 };
        //    modelBuilder.Entity<MachineType>().HasData(new[]
        //    {
        //        coffeeMachineBVM,
        //        frigoAndCoffeeMiniSnakky
        //    });


        //    // http://www.gesavending.it/it/catalogo-vending-machine?p=3
        //    var machine = new Machine(coffeeMachineBVM)
        //    {
        //        Id = 1,
        //        Status = true,
        //        Temperature = 35.44m,
        //        Position = new Services.Shared.Domain.MapPoint(45.4351m, 10.9988m),
        //    };

        //    modelBuilder.Entity<Machine>().HasData(new[]
        //    {
        //        machine
        //    });

        //    return modelBuilder;
        //}
    }
}
