using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Domain;
using static VendingMachine.Service.Machines.Domain.MachineType;

namespace VendingMachine.Service.Machines.Data.EntityConfigurations
{
    public class MachineEntityConfiguration : IEntityTypeConfiguration<Machine>
    {
        private const int _SRID = 4326;
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.MachineType);

            builder.Property(x => x.Position)
                .HasConversion(
                    v => SetPosition(v.X, v.Y),
                    v => new Services.Shared.Domain.MapPoint((decimal)v.X, (decimal)v.Y));

            builder
                .Property<DateTime?>("_dataCreated")
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
