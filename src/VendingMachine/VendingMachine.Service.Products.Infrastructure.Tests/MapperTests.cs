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
        private readonly IServiceCollection services;
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
        public void GrossPrice_Domain2ToEntity()
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
        public void ColdDrink_Domain2ToEntity()
        {
            var domain = new ColdDrink("acqua", 1.0m);
            domain.SetPrice(new GrossPrice(0.75m, 4));
            domain.SetTemperatureMaximun(100);
            domain.SetTemperatureMinimum(-5);

            var entity = mapper.Map<ColdDrinkEntity>(domain);
            Assert.Equal(100, entity.TemperatureMaximum);
            Assert.Equal(-5, entity.TemperatureMinimum);
            Assert.Equal("acqua", entity.Name);
            Assert.Equal(0.75m, entity.Litre);
        }
    }
}
