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
            //builder.Property(t => t.MachineStatus)
            //    .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
            //builder.Property(t => t.OrderProductItems)
            //    .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
            builder.Property(t => t.OrderDate);

            builder.Ignore(t => t.Billing);
            builder.OwnsOne(t => t.MachineStatus,
                c =>
                {
                    c.Property(x => x.MachineId).HasColumnName("MachineId");
                    c.Property(x => x.CoinsCurrentSupply).HasColumnName("CoinsCurrentSupply");
                }
            ).Ignore(t => t.MachineStatus);

            // https://docs.microsoft.com/it-it/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(t => t.OrderProductItems,
                 c => {
                    c.WithOwner().HasForeignKey(y => y.Id);
                    c.HasKey(t => t.ProductItemId);
                 }
            );
        }

        private EntityTypeBuilder<Order> SeedData(EntityTypeBuilder<Order> builder)
        {
            return builder;
        }
    }
}
