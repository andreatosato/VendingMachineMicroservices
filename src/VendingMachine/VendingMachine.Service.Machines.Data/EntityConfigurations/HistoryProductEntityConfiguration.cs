using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data.EntityConfigurations
{
    public class HistoryProductEntityConfiguration : IEntityTypeConfiguration<ProductConsumed>
    {
        public void Configure(EntityTypeBuilder<ProductConsumed> builder)
        {
            builder.ToTable("HistoryProducts");
            builder.HasKey(p => p.Id);
        }
    }
}
