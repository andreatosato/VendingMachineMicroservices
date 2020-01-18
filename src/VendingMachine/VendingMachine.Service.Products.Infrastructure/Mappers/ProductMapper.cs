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
                   .ForMember(domain => domain.Price, mapper => mapper.Ignore());

                cfg.CreateMap<ColdDrink, ColdDrinkEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, ctx) => new ColdDrink(entity.Name, ctx.Mapper.Map<GrossPrice>(entity.Price), entity.Litre))
                    .ForMember(domain => domain.TemperatureMaximum, mapper => mapper.Ignore())
                    .ForMember(domain => domain.TemperatureMinimum, mapper => mapper.Ignore())
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMaximun(entity.TemperatureMaximum))
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMinimum(entity.TemperatureMinimum));

                cfg.CreateMap<HotDrink, HotDrinkEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, ctx) => new HotDrink(entity.Name, ctx.Mapper.Map<GrossPrice>(entity.Price), entity.Grams))
                    .ForMember(domain => domain.TemperatureMaximum, mapper => mapper.Ignore())
                    .ForMember(domain => domain.TemperatureMinimum, mapper => mapper.Ignore())
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMaximun(entity.TemperatureMaximum))
                    .AfterMap((entity, domain, ctx) => domain.SetTemperatureMinimum(entity.TemperatureMinimum));

                cfg.CreateMap<Snack, SnackEntity>()
                    .IncludeBase<Product, ProductEntity>()
                    .ReverseMap()
                    .ConstructUsing((entity, ctx) => new Snack(entity.Name, ctx.Mapper.Map<GrossPrice>(entity.Price), entity.Grams));

                cfg.CreateMap<ProductItem, ProductItemEntity>()
                   .ReverseMap()
                   .ConstructUsing((entity, ctx) =>
                   {
                       if (entity.Product is ColdDrinkEntity coldDrinkEntity)
                       {
                           return new ProductItem(new ColdDrink(coldDrinkEntity.Name, ctx.Mapper.Map<GrossPrice>(coldDrinkEntity.Price), coldDrinkEntity.Litre));
                       }
                       if (entity.Product is HotDrinkEntity hotDrinkEntity)
                       {
                           return new ProductItem(new HotDrink(hotDrinkEntity.Name, ctx.Mapper.Map<GrossPrice>(hotDrinkEntity.Price), hotDrinkEntity.Grams));
                       }
                       if (entity.Product is SnackEntity snakEntity)
                       {
                           return new ProductItem(new Snack(snakEntity.Name, ctx.Mapper.Map<GrossPrice>(snakEntity.Price), snakEntity.Grams));
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
                       domain.SetExpirationDate(entity.ExpirationDate);
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
