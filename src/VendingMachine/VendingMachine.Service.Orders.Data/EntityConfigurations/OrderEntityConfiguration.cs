using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendingMachine.Service.Orders.Domain;

namespace VendingMachine.Service.Orders.Data.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(t => t.OrderDate);
            builder.Property(t => t.Processed);
            builder.Ignore(t => t.Billing);

            builder.OwnsOne(t => t.MachineStatus,
                c =>
                {
                    // Owns Type in same table
                    c.ToTable("Orders");
                    // Avoid Table_Property convention
                    c.Property(x => x.MachineId).HasColumnName("MachineId");
                    c.Property(x => x.CoinsCurrentSupply).HasColumnName("CoinsCurrentSupply");
                }
            );

            // https://docs.microsoft.com/it-it/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(t => t.OrderProductItems,
                 c =>
                 {
                     c.WithOwner().HasForeignKey("OrderId");
                     c.HasKey(t => t.Id);
                     c.Property(t => t.Id).ValueGeneratedOnAdd();
                     c.Property(t => t.ProductItemId);
                     c.OwnsOne(t => t.Price,
                         p =>
                         {
                             // Avoid Table_Property convention
                             p.Property(x => x.Value).HasColumnName("GrossPrice");
                             p.Property(x => x.TaxPercentage).HasColumnName("TaxPercentage");
                             p.Property(x => x.NetPrice).HasColumnName("NetPrice");
                             p.Property(x => x.Rate).HasColumnName("Rate");
                         });
                 }
            );
        }

        private EntityTypeBuilder<Order> SeedData(EntityTypeBuilder<Order> builder)
        {
            return builder;
        }
    }
}
