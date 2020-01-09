using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data.EntityConfigurations
{
    public class MachineEntityConfiguration : IEntityTypeConfiguration<MachineItem>
    {
        private const int _SRID = 4326;
        public void Configure(EntityTypeBuilder<MachineItem> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.MachineType);
            builder.HasMany(p => p.ActiveProducts);
            builder.HasMany(p => p.HistoryProducts);

            builder.Property(x => x.Position)
                .HasConversion(
                    v => SetPosition(v.X, v.Y),
                    v => new Shared.Domain.MapPoint((decimal)v.X, (decimal)v.Y));

            builder
                .Property<DateTime>("_dataCreated")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("DataCreated")
                .IsRequired();

            builder
               .Property<DateTime?>("_dataUpdated")
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("DataUpdated")
               .IsRequired(false);
        }

        public Point SetPosition(decimal x, decimal y)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: _SRID);
            return geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate((double)x, (double)y));
        }
    }
}
