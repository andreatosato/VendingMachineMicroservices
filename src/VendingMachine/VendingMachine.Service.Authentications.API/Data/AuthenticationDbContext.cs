using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VendingMachine.Service.Authentications.API.Data.Models;

namespace VendingMachine.Service.Authentications.API.Data
{
    public class AuthenticationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AuthenticationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeds Data
            string user1Mail = "andrea.tosato@4ward.it";
            var user1Object = new ApplicationUser()
            {
                Email = user1Mail,
                EmailConfirmed = true,
                FirstName = "Andrea",
                LastName = "Tosato",
                LockoutEnabled = false,
                UserName = user1Mail,                
            };
            user1Object.Id = Guid.NewGuid();
            user1Object.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user1Object, "Pass123$");

            modelBuilder.Entity<ApplicationUser>()
                .HasData(user1Object);


            var userRole = new ApplicationRole("User")
            {
                Id = Guid.NewGuid()
            };
            var adminRole = new ApplicationRole("Admin")
            {
                Id = Guid.NewGuid()
            };
            modelBuilder.Entity<ApplicationRole>()
                .HasData(userRole, adminRole);

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>() { UserId = user1Object.Id, RoleId = adminRole.Id });
        }
    }
}
