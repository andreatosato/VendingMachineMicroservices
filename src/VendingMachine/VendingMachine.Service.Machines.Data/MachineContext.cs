using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data.EntityConfigurations;
using VendingMachine.Service.Machines.Data.Seeds;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Shared.Data;
using VendingMachine.Service.Shared.Domain;

namespace VendingMachine.Service.Machines.Data
{
    public class MachineContext : DbContext
    {
        private readonly IMediator _mediator;
        public DbSet<MachineItem> Machines { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<Product> ActiveProduct { get; set; }
        public DbSet<ProductConsumed> HistoryProduct { get; set; }

        public MachineContext()
        {
        }

        // Only for application
        public MachineContext(DbContextOptions<MachineContext> options, IMediator _mediator)
            : base(options)
        {
            this._mediator = _mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new MachineEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MachineVersionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ActiveProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryProductEntityConfiguration());

            // Choose manual migration
            // modelBuilder.SeedData();      
            AddBorgoRoma(modelBuilder);
            AddBorgoTrento(modelBuilder);
        }


        private void AddBorgoRoma(ModelBuilder modelBuilder)
        {
            var productItem1 = new
            {
                Id = 1,
                MachineItemId = 1,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };

            var productItem2 = new
            {
                Id = 2,
                MachineItemId = 1,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };
            var productItem3 = new
            {
                Id = 3,
                MachineItemId = 1,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };
            var productItem4 = new
            {
                Id = 4,
                MachineItemId = 1,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                ProvidedDate = new DateTimeOffset(2020, 2, 13, 0, 0, 0, TimeSpan.Zero),
            };           
            var productItem5 = new
            {
                Id = 5,
                MachineItemId = 1,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                ProvidedDate = new DateTimeOffset(2020, 2, 13, 0, 0, 0, TimeSpan.Zero),
            };

            var machineType = new
            {
                Id = 1,
                Model = "BVM",
                Version = MachineType.MachineVersion.Coffee
            };
            modelBuilder.Entity<MachineType>().HasData(machineType);

            var machine1 = new
            {
                Id = 1,
                MachineTypeId = 1,
                // ActiveProducts = new[] { productItem1 },
                // HistoryProducts = new[] { productItem4, productItem5 },
                Position = new MapPoint(10.9928686m, 45.4041261m), //SeedBase.SetPosition(45.4351m, 10.9988m),
                Temperature = 22.3m,
                Status = true,
                // MachineType = machineType,
                LatestLoadedProducts = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                LatestCleaningMachine = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                LatestMoneyCollection = new DateTimeOffset(2020, 1, 30, 0, 0, 0, TimeSpan.Zero),
                CoinsInMachine = 15.55m,
                CoinsCurrentSupply = 0m,
                _dataCreated = DateTime.UtcNow                
            };

            modelBuilder.Entity<Product>().HasData(productItem1);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem1);
            modelBuilder.Entity<Product>().HasData(productItem2);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem2);
            modelBuilder.Entity<Product>().HasData(productItem3);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem3);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem4);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem5);
            modelBuilder.Entity<MachineItem>().HasData(machine1);
        }

        private void AddBorgoTrento(ModelBuilder modelBuilder)
        {           
            var productItem6 = new
            {
                Id = 6,
                MachineItemId = 2,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };

            var productItem7 = new
            {
                Id = 7,
                MachineItemId = 2,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };
            var productItem8 = new
            {
                Id = 8,
                MachineItemId = 2,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
            };
            var productItem9 = new
            {
                Id = 9,
                MachineItemId = 2,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                ProvidedDate = new DateTimeOffset(2020, 2, 13, 0, 0, 0, TimeSpan.Zero),
            };
            var productItem10 = new
            {
                Id = 10,
                MachineItemId = 2,
                ActivationDate = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                ProvidedDate = new DateTimeOffset(2020, 2, 13, 0, 0, 0, TimeSpan.Zero),
            };

            var machineTypeFrigo = new
            {
                Id = 2,
                Model = "MiniSnakky",
                Version = MachineType.MachineVersion.FrigoAndCoffee
            };
            modelBuilder.Entity<MachineType>().HasData(machineTypeFrigo);

            var machine2 = new
            {
                Id = 2,
                MachineTypeId = 2,
                // ActiveProducts = new[] { productItem1 },
                // HistoryProducts = new[] { productItem4, productItem5 },
                Position = new MapPoint(10.9839598m, 45.4518442m), //SeedBase.SetPosition(45.4351m, 10.9988m),
                Temperature = 22.3m,
                Status = true,
                // MachineType = machineType,
                LatestLoadedProducts = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                LatestCleaningMachine = new DateTimeOffset(2020, 2, 10, 0, 0, 0, TimeSpan.Zero),
                LatestMoneyCollection = new DateTimeOffset(2020, 1, 30, 0, 0, 0, TimeSpan.Zero),
                CoinsInMachine = 15.55m,
                CoinsCurrentSupply = 0m,
                _dataCreated = DateTime.UtcNow
            };

            modelBuilder.Entity<Product>().HasData(productItem6);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem6);
            modelBuilder.Entity<Product>().HasData(productItem7);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem7);
            modelBuilder.Entity<Product>().HasData(productItem8);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem8);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem9);
            modelBuilder.Entity<ProductConsumed>().HasData(productItem10);
            modelBuilder.Entity<MachineItem>().HasData(machine2);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);
            await _mediator?.DispatchDomainEventsAsync(this);
            return result;
        }
    }


    public class MachineContextFactory : IDesignTimeDbContextFactory<MachineContext>
    {
        public MachineContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MachineContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=VendingMachine-Machines;Trusted_Connection=True;MultipleActiveResultSets=true",
                x => x.UseNetTopologySuite());
            optionsBuilder.EnableSensitiveDataLogging();

            return new MachineContext(optionsBuilder.Options, null);
        }
    }
}
