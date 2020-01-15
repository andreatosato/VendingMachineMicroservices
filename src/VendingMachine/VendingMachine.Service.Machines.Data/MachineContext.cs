using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Service.Machines.Data.EntityConfigurations;
using VendingMachine.Service.Machines.Domain;
using VendingMachine.Service.Shared.Data;

namespace VendingMachine.Service.Machines.Data
{
    public class MachineContext : DbContext
    {
        private readonly IMediator _mediator;
        public DbSet<MachineItem> Machines { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<Product> ActiveProduct { get; set; }
        public DbSet<ProductConsumed> HistoryProduct { get; set; }

        public MachineContext()
        {
        }

        // Only for application
        public MachineContext(DbContextOptions<MachineContext> options, IMediator _mediator)
            : base(options)
        {
            this._mediator = _mediator;
        }

        // Only for test
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=VendingMachine-Machines;Trusted_Connection=True;MultipleActiveResultSets=true",
                x => x.UseNetTopologySuite());
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MachineEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MachineVersionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ActiveProductEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryProductEntityConfiguration());

            // Choose manual migration
            //modelBuilder.SeedData();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);
            await _mediator?.DispatchDomainEventsAsync(this);
            return result;
        }
    }
}
