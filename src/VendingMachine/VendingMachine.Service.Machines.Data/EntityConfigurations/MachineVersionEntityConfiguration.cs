using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Domain;
using static VendingMachine.Service.Machines.Domain.MachineType;

namespace VendingMachine.Service.Machines.Data.EntityConfigurations
{
    public class MachineVersionEntityConfiguration : IEntityTypeConfiguration<MachineType>
    {
        public void Configure(EntityTypeBuilder<MachineType> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(x => x.Model)
                   .HasMaxLength(255);

            builder.Property(c => c.Version)
                .HasConversion(
                    v => v.ToString(),
                    v => (MachineVersion)Enum.Parse(typeof(MachineVersion), v)
                );

            builder.HasIndex(x => new { x.Model, x.Version })
                .HasName("IX_UniqueMachineVersion")
                .IsUnique();
        }
    }
}
