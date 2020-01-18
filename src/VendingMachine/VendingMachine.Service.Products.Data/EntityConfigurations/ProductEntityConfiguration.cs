using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendingMachine.Service.Products.Data.Entities;

namespace VendingMachine.Service.Products.Data.EntityConfigurations
{
    // Table per Hierarchy (TPH)
    // https://entityframeworkcore.com/model-inheritance
    // https://docs.microsoft.com/en-us/ef/core/modeling/inheritance

    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Version).IsRowVersion().HasColumnName("Version");

            //owned types with table splitting
            // https://docs.microsoft.com/en-us/ef/core/modeling/table-splitting
            builder.OwnsOne(
                o => o.Price,
                sa =>
                {
                    sa.Property(p => p.GrossPrice).HasColumnName("GrossPrice");
                    sa.Property(p => p.TaxPercentage).HasColumnName("TaxPercentage");
                    sa.Property(p => p.NetPrice).HasColumnName("NetPrice");
                    sa.Property(p => p.Rate).HasColumnName("Rate");
                });
        }
    }

    public class ColdDrinkEntityConfiguration : IEntityTypeConfiguration<ColdDrinkEntity>
    {
        public void Configure(EntityTypeBuilder<ColdDrinkEntity> builder)
        {
            builder.HasBaseType<ProductEntity>();
            builder.Property(x => x.TemperatureMaximum).HasColumnName("TemperatureMax");
            builder.Property(x => x.TemperatureMinimum).HasColumnName("TemperatureMinimum");
            
            //owned types with table splitting
            builder.OwnsOne(
               o => o.Price,
               sa =>
               {
                   sa.Property(p => p.GrossPrice).HasColumnName("GrossPrice");
                   sa.Property(p => p.TaxPercentage).HasColumnName("TaxPercentage");
                   sa.Property(p => p.NetPrice).HasColumnName("NetPrice");
                   sa.Property(p => p.Rate).HasColumnName("Rate");
               });
        }
    }

    public class HotDrinkEntityConfiguration : IEntityTypeConfiguration<HotDrinkEntity>
    {
        public void Configure(EntityTypeBuilder<HotDrinkEntity> builder)
        {
            builder.HasBaseType<ProductEntity>();
            builder.Property(x => x.TemperatureMaximum).HasColumnName("TemperatureMax");
            builder.Property(x => x.TemperatureMinimum).HasColumnName("TemperatureMinimum");
            builder.Property(x => x.Grams).HasColumnName("Grams");

            //owned types with table splitting
            builder.OwnsOne(
               o => o.Price,
               sa =>
               {
                   sa.Property(p => p.GrossPrice).HasColumnName("GrossPrice");
                   sa.Property(p => p.TaxPercentage).HasColumnName("TaxPercentage");
                   sa.Property(p => p.NetPrice).HasColumnName("NetPrice");
                   sa.Property(p => p.Rate).HasColumnName("Rate");
               });
        }
    }

    public class SnakEntityConfiguration : IEntityTypeConfiguration<SnakEntity>
    {
        public void Configure(EntityTypeBuilder<SnakEntity> builder)
        {
            builder.HasBaseType<ProductEntity>();
            builder.Property(x => x.Grams).HasColumnName("Grams");

            //owned types with table splitting
            builder.OwnsOne(
               o => o.Price,
               sa =>
               {
                   sa.Property(p => p.GrossPrice).HasColumnName("GrossPrice");
                   sa.Property(p => p.TaxPercentage).HasColumnName("TaxPercentage");
                   sa.Property(p => p.NetPrice).HasColumnName("NetPrice");
                   sa.Property(p => p.Rate).HasColumnName("Rate");
               });
        }
    }
}
