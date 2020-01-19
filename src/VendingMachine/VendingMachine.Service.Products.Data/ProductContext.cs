using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using VendingMachine.Service.Products.Data.Entities;
using VendingMachine.Service.Products.Data.EntityConfigurations;

namespace VendingMachine.Service.Products.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductItemEntity> ProductItems { get; set; }

        public ProductContext()
        {
        }

        public ProductContext([NotNull] DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=VendingMachine-Products;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ColdDrinkEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HotDrinkEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SnakEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductItemEntityConfiguration());
            
            var cold = new ColdDrinkEntity()
            {
                Name = "Acqua",
                Litre = 0.5m,
                TemperatureMaximum = 35,
                TemperatureMinimum = 0,
                Id = 1
            };
            var hot = new HotDrinkEntity()
            {
                Name = "Caffè",
                Grams = 7,
                TemperatureMaximum = 25,
                TemperatureMinimum = 15,
                Id = 2
            };
            var snack = new SnackEntity()
            {
                Name = "Kinder Delice",
                Grams = 40,
                Id = 3
            };

            var kinderDelice = new
            {
                Id = 1,
                ProductId = snack.Id,
                ExpirationDate = new System.DateTime(2022, 1, 1, 0, 0, 0),
                Purchased = new System.DateTimeOffset(2020, 1, 6, 9, 0, 0, System.TimeSpan.Zero),
                Sold = new System.DateTimeOffset(2020, 1, 6, 11, 0, 0, System.TimeSpan.Zero),
            };

            modelBuilder.Entity<ColdDrinkEntity>().HasData(cold);
            modelBuilder.Entity<ColdDrinkEntity>().OwnsOne(p => p.Price).HasData(new 
            {
                ProductEntityId = cold.Id,
                GrossPrice = 0.50m,
                TaxPercentage = 4,
                NetPrice = 0.48m,
                Rate = 0.02m
            });
            modelBuilder.Entity<HotDrinkEntity>().HasData(hot);
            modelBuilder.Entity<HotDrinkEntity>().OwnsOne(p => p.Price).HasData(new
            {
                ProductEntityId = hot.Id,
                GrossPrice = 0.50m,
                TaxPercentage = 4,
                NetPrice = 0.48m,
                Rate = 0.02m
            });
            modelBuilder.Entity<SnackEntity>().HasData(snack);
            modelBuilder.Entity<SnackEntity>().OwnsOne(p => p.Price).HasData(new
            {
                ProductEntityId = snack.Id,
                GrossPrice = 0.70m,
                TaxPercentage = 4,
                NetPrice = 0.67m,
                Rate = 0.03m
            });


            modelBuilder.Entity<ProductItemEntity>().HasData(kinderDelice);
            modelBuilder.Entity<ProductItemEntity>().OwnsOne(p => p.SoldPrice).HasData(new
            {
                ProductItemEntityId = kinderDelice.Id,
                GrossPrice = 0.90m,
                TaxPercentage = 4,
                NetPrice = 0.86m,
                Rate = 0.04m
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
