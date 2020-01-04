using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data.EntityConfigurations
{
    public class ActiveProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("ActiveProducts");
            builder.HasKey(p => p.Id);
        }
    }
}
