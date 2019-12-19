using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data
{
    public static class MigrationExtension
    {
        
        public static ModelBuilder SeedData(this ModelBuilder modelBuilder)
        {
            var coffeeMachineBVM = new MachineType("BVM", MachineType.MachineVersion.Coffee);
            var frigoAndCoffeeMiniSnakky = new MachineType("MiniSnakky", MachineType.MachineVersion.FrigoAndCoffee);
            modelBuilder.Entity<MachineType>().HasData(new[]
            {
                coffeeMachineBVM,
                frigoAndCoffeeMiniSnakky
            });


            // http://www.gesavending.it/it/catalogo-vending-machine?p=3
            var machine = new Machine(coffeeMachineBVM)
            {
                Id = 1,
                Status = true,
                Temperature = 35.44m,
                Position = new Services.Shared.Domain.MapPoint(45.4351m, 10.9988m)
            };

            modelBuilder.Entity<Machine>().HasData(new[]
            {
                machine
            });

            return modelBuilder;
        }
    }
}
