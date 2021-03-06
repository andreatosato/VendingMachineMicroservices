﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VendingMachine.Service.Authentications.API.Data;

namespace VendingMachine.Service.Authentications.API.Migrations
{
    [DbContext(typeof(AuthenticationDbContext))]
    [Migration("20200213075255_SecondMigrationAuth")]
    partial class SecondMigrationAuth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Machine.Api",
                            UserId = new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Product.Api",
                            UserId = new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319")
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Order.Api",
                            UserId = new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319")
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Machine.Api",
                            UserId = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a")
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Product.Api",
                            UserId = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a")
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "VendingMachineApiClaim",
                            ClaimValue = "Order.Api",
                            UserId = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"),
                            RoleId = new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4")
                        },
                        new
                        {
                            UserId = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"),
                            RoleId = new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4")
                        },
                        new
                        {
                            UserId = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"),
                            RoleId = new Guid("f4b49977-b119-4764-a317-b321dc80be7c")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("VendingMachine.Service.Authentications.API.Data.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f4b49977-b119-4764-a317-b321dc80be7c"),
                            ConcurrencyStamp = "b6f7c68e-5f24-4967-83fe-bc5eea69fb1e",
                            Name = "User"
                        },
                        new
                        {
                            Id = new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4"),
                            ConcurrencyStamp = "1ff59ed3-50a8-49e2-b0b2-d4e508d96b6f",
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Authentications.API.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "ace209b6-940a-472a-8326-20328d991df8",
                            Email = "andrea.tosato@4ward.it",
                            EmailConfirmed = true,
                            FirstName = "Andrea",
                            LastName = "Tosato",
                            LockoutEnabled = false,
                            NormalizedEmail = "andrea.tosato@4ward.it",
                            NormalizedUserName = "andrea.tosato@4ward.it",
                            PasswordHash = "AQAAAAEAACcQAAAAEBZvFdXTRYmqm6Bz4j5rUCQITBE+NC1pPrd1+39aFprFdOxcldCDG9jFuQi0jxZGsQ==",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "andrea.tosato@4ward.it"
                        },
                        new
                        {
                            Id = new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "d9f54081-5a89-4706-9703-dde7d5295a0e",
                            Email = "test@cloudgen.it",
                            EmailConfirmed = true,
                            FirstName = "Associazione",
                            LastName = "Cloudgen",
                            LockoutEnabled = false,
                            NormalizedEmail = "test@cloudgen.it",
                            NormalizedUserName = "test@cloudgen.it",
                            PasswordHash = "AQAAAAEAACcQAAAAEM3qFS6FxNJF9cKePibhEw22rUPra26LqDTSPP4i/MOFKXHvEWIrQD7xrsBs/98jIw==",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            UserName = "test@cloudgen.it"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("VendingMachine.Service.Authentications.API.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
