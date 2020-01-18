using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendingMachine.Service.Products.Data.Entities;

namespace VendingMachine.Service.Products.Data.EntityConfigurations
{
    public class ProductItemEntityConfiguration : IEntityTypeConfiguration<ProductItemEntity>
    {
        public void Configure(EntityTypeBuilder<ProductItemEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Product);
            builder.OwnsOne(
                o => o.SoldPrice,
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
