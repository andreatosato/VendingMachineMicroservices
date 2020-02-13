using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VendingMachine.Service.Authentications.API.Data.Models;
using VendingMachine.Service.Shared.Authentication;

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
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            string user1Mail = "andrea.tosato@4ward.it";
            var user1Object = new ApplicationUser()
            {
                Email = user1Mail,
                NormalizedEmail = user1Mail,
                EmailConfirmed = true,
                FirstName = "Andrea",
                LastName = "Tosato",
                LockoutEnabled = false,
                UserName = user1Mail,
                NormalizedUserName = user1Mail
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

            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 1, UserId = user1Object.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.MachineApi });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 2, UserId = user1Object.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.ProductApi });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 3, UserId = user1Object.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.OrderApi });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>() { UserId = user1Object.Id, RoleId = adminRole.Id });







            string userWorkshopMail = "test@cloudgen.it";
            var userWorksObject = new ApplicationUser()
            {
                Email = userWorkshopMail,
                NormalizedEmail = userWorkshopMail,
                EmailConfirmed = true,
                FirstName = "Associazione",
                LastName = "Cloudgen",
                LockoutEnabled = false,
                UserName = userWorkshopMail,
                NormalizedUserName = userWorkshopMail
            };
            userWorksObject.Id = Guid.NewGuid();
            userWorksObject.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(userWorksObject, "Pass123$");

            modelBuilder.Entity<ApplicationUser>()
                .HasData(userWorksObject);


           
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 4, UserId = userWorksObject.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.MachineApi });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 5, UserId = userWorksObject.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.ProductApi });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasData(new IdentityUserClaim<Guid>() { Id = 6, UserId = userWorksObject.Id, ClaimType = VendingMachineClaimTypes.ApiClaim, ClaimValue = VendingMachineClaimValues.OrderApi });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>() { UserId = userWorksObject.Id, RoleId = adminRole.Id });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>() { UserId = userWorksObject.Id, RoleId = userRole.Id });

        }
    }
}
