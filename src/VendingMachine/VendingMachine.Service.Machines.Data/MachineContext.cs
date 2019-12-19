using Microsoft.EntityFrameworkCore;
using VendingMachine.Service.Machines.Data.EntityConfigurations;
using VendingMachine.Service.Machines.Domain;

namespace VendingMachine.Service.Machines.Data
{
    public class MachineContext : DbContext
    {
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }

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
            modelBuilder.SeedData();
        }
    }
}
