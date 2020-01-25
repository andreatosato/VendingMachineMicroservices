using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service.Orders.Data.EntityConfigurations;
using VendingMachine.Service.Orders.Domain;

namespace VendingMachine.Service.Orders.Data
{
    public class OrderContext : DbContext
    {
        private readonly IMediator _mediator;
        public DbSet<Order> Orders { get; set; }

        protected OrderContext()
        {
        }

        public OrderContext(DbContextOptions<OrderContext> options, IMediator _mediator)
            : base(options)
        {
            this._mediator = _mediator;
        }

        // Look Design Time Down in the file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }


    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=VendingMachine-Orders;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.EnableSensitiveDataLogging();

            return new OrderContext(optionsBuilder.Options, null);
        }
    }
}
