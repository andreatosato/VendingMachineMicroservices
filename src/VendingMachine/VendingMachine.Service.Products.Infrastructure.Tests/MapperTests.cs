using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using VendingMachine.Service.Products.Data.Entities;
using VendingMachine.Service.Products.Domain;
using Xunit;

namespace VendingMachine.Service.Products.Infrastructure.Tests
{
    public class MapperTests
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;
        public MapperTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMapper();
            serviceProvider = services.BuildServiceProvider();
            mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        [Fact]
        public void GrossPrice_DomainToEntity()
        {
            var domain = new GrossPrice(0.75m, 21);          

            var entity = mapper.Map<GrossPriceEntity>(domain);
            Assert.Equal(0.75m, entity.GrossPrice);
            Assert.Equal(21, entity.TaxPercentage);
            Assert.Equal(0.5925m, entity.NetPrice);
            Assert.Equal(0.1575m, entity.Rate);

            domain.RedefinePrice(0.85m);
            entity = mapper.Map<GrossPriceEntity>(domain);
            Assert.Equal(0.85m, entity.GrossPrice);
            Assert.Equal(21, entity.TaxPercentage);
            Assert.Equal(0.6715m, entity.NetPrice);
            Assert.Equal(0.1785m, entity.Rate);
        }

        [Fact]
        public void GrossPrice_EntityToDomain()
        {
            var entity = new GrossPriceEntity()
            {
                GrossPrice = 0.75m,
                NetPrice = 0.5925m,
                Rate = 0.1575m,
                TaxPercentage = 21
            };
            var domain = mapper.Map<GrossPrice>(entity);

            Assert.Equal(0.75m, domain.Value);
            Assert.Equal(21, domain.TaxPercentage);
            Assert.Equal(0.5925m, domain.NetPrice);
            Assert.Equal(0.1575m, domain.Rate);
        }

        [Fact]
        public void ColdDrink_DomainToEntity()
        {
            var domain = new ColdDrink("acqua", new GrossPrice(0.75m, 4), 1.5m);
            domain.SetTemperatureMaximun(100);
            domain.SetTemperatureMinimum(-5);

            var entity = mapper.Map<ColdDrinkEntity>(domain);
            Assert.Equal(100, entity.TemperatureMaximum);
            Assert.Equal(-5, entity.TemperatureMinimum);
            Assert.Equal("acqua", entity.Name);
            Assert.Equal(1.5m, entity.Litre);
        }

        [Fact]
        public void ColdDrink_EntityToDomain()
        {
            var entity = new ColdDrinkEntity()
            {
                Litre = 1.5m,
                Name = "acqua",
                Price = new GrossPriceEntity() { GrossPrice = 0.75m, TaxPercentage = 4 },
                TemperatureMaximum = 100,
                TemperatureMinimum = -5,
            };
            var domain = mapper.Map<ColdDrink>(entity);
            Assert.Equal(100, domain.TemperatureMaximum);
            Assert.Equal(-5, domain.TemperatureMinimum);
            Assert.Equal("acqua", domain.Name);
            Assert.Equal(1.5m, domain.Litre);
        }


        [Fact]
        public void HotDrink_DomainToEntity()
        {
            var domain = new HotDrink("coffee", new GrossPrice(0.75m, 4), 25.7m);
            domain.SetTemperatureMaximun(35);
            domain.SetTemperatureMinimum(15);

            var entity = mapper.Map<HotDrinkEntity>(domain);
            Assert.Equal(35, entity.TemperatureMaximum);
            Assert.Equal(15, entity.TemperatureMinimum);
            Assert.Equal("coffee", entity.Name);
            Assert.Equal(25.7m, entity.Grams);
        }

        [Fact]
        public void HotDrink_EntityToDomain()
        {
            var entity = new HotDrinkEntity()
            {
                Grams = 25.7m,
                Name = "coffee",
                Price = new GrossPriceEntity() { GrossPrice = 0.75m, TaxPercentage = 4 },
                TemperatureMaximum = 35,
                TemperatureMinimum = 15,
            };
            var domain = mapper.Map<HotDrink>(entity);
            Assert.Equal(35, domain.TemperatureMaximum);
            Assert.Equal(15, domain.TemperatureMinimum);
            Assert.Equal("coffee", domain.Name);
            Assert.Equal(25.7m, domain.Grams);
        }

        [Fact]
        public void Snak_DomainToEntity()
        {
            var domain = new Snak("kinder delice", new GrossPrice(0.75m, 4), 39);

            var entity = mapper.Map<SnakEntity>(domain);
            Assert.Equal(39, entity.Grams);
            Assert.Equal("kinder delice", entity.Name);
        }

        [Fact]
        public void Snak_EntityToDomain()
        {
            var entity = new SnakEntity()
            {
                Grams = 39,
                Name = "kinder delice",
                Price = new GrossPriceEntity() { GrossPrice = 0.75m, TaxPercentage = 4 },
            };

            var domain = mapper.Map<Snak>(entity);
            Assert.Equal(39, domain.Grams);
            Assert.Equal("kinder delice", domain.Name);
        }



        [Fact]
        public void ProductItem_DomainToEntity()
        {
            var kinderDelice = new Snak("kinder delice", new GrossPrice(0.75m, 4), 39);

            var domain = new ProductItem(kinderDelice);
            domain.SetExpirationDate(new DateTime(2022, 1, 31));
            domain.SetGrossPriceValue(0.75m);
            domain.SetPurchasedDate(new DateTimeOffset(2020, 1, 31, 9, 0, 0, TimeSpan.Zero));
            domain.SetSoldDate(new DateTimeOffset(2020, 2, 14, 9, 0, 0, TimeSpan.Zero));

            var entity = mapper.Map<ProductItemEntity>(domain);
            Assert.Equal(new DateTime(2022, 1, 31), entity.ExpirationDate);
            Assert.Equal(new DateTimeOffset(2020, 1, 31, 9, 0, 0, TimeSpan.Zero), entity.Purchased);
            Assert.Equal(new DateTimeOffset(2020, 2, 14, 9, 0, 0, TimeSpan.Zero), entity.Sold);
            Assert.Equal(new DateTime(2022, 1, 31), entity.ExpirationDate);
            Assert.Equal(kinderDelice.Name, entity.Product.Name);
        }

        [Fact]
        public void ProductItem_EntityToDomain()
        {
            var entity = new ProductItemEntity()
            {
                Product = new SnakEntity()
                {
                    Grams = 39,
                    Name = "kinder delice",
                    Price = new GrossPriceEntity() { GrossPrice = 0.75m, TaxPercentage = 4 },
                },
                Sold = new DateTimeOffset(2020, 2, 14, 9, 0, 0, TimeSpan.Zero),
                Purchased = new DateTimeOffset(2020, 1, 31, 9, 0, 0, TimeSpan.Zero),
                ExpirationDate = new DateTime(2022, 1, 31),
                SoldPrice = new GrossPriceEntity() { GrossPrice = 0.75m, TaxPercentage = 4 }
            };

            var domain = mapper.Map<ProductItem>(entity);
            Assert.Equal(new DateTime(2022, 1, 31), domain.ExpirationDate);
            Assert.Equal(new DateTimeOffset(2020, 1, 31, 9, 0, 0, TimeSpan.Zero), domain.Purchased);
            Assert.Equal(new DateTimeOffset(2020, 2, 14, 9, 0, 0, TimeSpan.Zero), domain.Sold);
            Assert.Equal(0.75m, domain.SoldPrice.Value);
        }
    }
}
