using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using VendingMachine.Service.Products.Data.Entities;
using VendingMachine.Service.Products.Domain;
using VendingMachine.Service.Products.Infrastructure.Mappers;

namespace VendingMachine.Service.Products.Infrastructure.Mappers
{
    public static class ProductMapper
    {
        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GrossPrice, GrossPriceEntity>()
                   .ForMember(entity => entity.GrossPrice, mapper => mapper.MapFrom(domain => domain.Value))
                   .ReverseMap()
                   .ConstructUsing((entity, domain) => new GrossPrice(entity.GrossPrice, entity.TaxPercentage))
                   .ForMember(domain => domain.Value, mapper => mapper.Ignore())
                   .ForMember(domain => domain.TaxPercentage, mapper => mapper.Ignore());


                cfg.CreateMap<Product, ProductEntity>()
                   .ForMember(entity => entity.Price, mapper => mapper.MapFrom(entity => entity.Price))
                   .ReverseMap()
                   .ForMember(domain => domain.Price, mapper => mapper.Ignore())
                   .AfterMap((entity, domain, ctx) => domain.SetPrice(ctx.Mapper.Map<GrossPrice>(entity.Price)));

                cfg.CreateMap<ColdDrink, ColdDrinkEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, domain) => new ColdDrink(entity.Name, entity.Litre))
                    .ForMember(domain => domain.TemperatureMaximum, mapper => mapper.Ignore())
                    .ForMember(domain => domain.TemperatureMinimum, mapper => mapper.Ignore())
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMaximun(entity.TemperatureMaximum))
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMinimum(entity.TemperatureMinimum));

                cfg.CreateMap<HotDrink, HotDrinkEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, domain) => new HotDrink(entity.Name, entity.Grams))
                    .ForMember(domain => domain.TemperatureMaximum, mapper => mapper.Ignore())
                    .ForMember(domain => domain.TemperatureMinimum, mapper => mapper.Ignore())
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMaximun(entity.TemperatureMaximum))
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMinimum(entity.TemperatureMinimum));

                cfg.CreateMap<Snak, SnakEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, domain) => new Snak(entity.Name, entity.Grams));

                cfg.CreateMap<ProductItem, ProductItemEntity>()
                   .ReverseMap()
                   .ConstructUsing((entity, domain) =>
                   {
                       if (entity.Product is ColdDrinkEntity coldDrinkEntity)
                       {
                           return new ProductItem(new ColdDrink(coldDrinkEntity.Name, coldDrinkEntity.Litre));
                       }
                       if (entity.Product is HotDrinkEntity hotDrinkEntity)
                       {
                           return new ProductItem(new HotDrink(hotDrinkEntity.Name, hotDrinkEntity.Grams));
                       }
                       if (entity.Product is SnakEntity snakEntity)
                       {
                           return new ProductItem(new Snak(snakEntity.Name, snakEntity.Grams));
                       }
                       throw new InvalidCastException("Can't create a valid product");
                   })
                   .ForMember(domain => domain.ExpirationDate, mapper => mapper.Ignore())
                   .ForMember(domain => domain.SoldPrice, mapper => mapper.Ignore())
                   .ForMember(domain => domain.Sold, mapper => mapper.Ignore())
                   .ForMember(domain => domain.Purchased, mapper => mapper.Ignore())
                   .AfterMap((entity, domain, ctx) =>
                   {
                       if (entity.SoldPrice != null)
                           domain.SetGrossPriceValue(entity.SoldPrice.GrossPrice);
                       if (entity.Purchased.HasValue)
                           domain.SetPurchasedDate(entity.Purchased.Value);
                       if (entity.Sold.HasValue)
                           domain.SetSoldDate(entity.Sold.Value);
                   });
            });

            return configuration.CreateMapper();
        }
    }
}

namespace VendingMachine.Service.Products
{
    public static class ProductMapperExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(s => ProductMapper.GetMapper());
            return services;
        }
    }
}
