﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using VendingMachine.Service.Machines.Data;

namespace VendingMachine.Service.Machines.Data.Migrations
{
    [DbContext(typeof(MachineContext))]
    [Migration("20200213101855_FirstMigrationMachines")]
    partial class FirstMigrationMachines
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.MachineItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CoinsCurrentSupply")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CoinsInMachine")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset?>("LatestCleaningMachine")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LatestLoadedProducts")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LatestMoneyCollection")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MachineTypeId")
                        .HasColumnType("int");

                    b.Property<Point>("Position")
                        .HasColumnType("geography");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal?>("Temperature")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("_dataCreated")
                        .HasColumnName("DataCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("_dataUpdated")
                        .HasColumnName("DataUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MachineTypeId");

                    b.ToTable("Machines");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CoinsCurrentSupply = 0m,
                            CoinsInMachine = 15.55m,
                            LatestCleaningMachine = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LatestLoadedProducts = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LatestMoneyCollection = new DateTimeOffset(new DateTime(2020, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineTypeId = 1,
                            Position = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (10.9928686 45.4041261)"),
                            Status = true,
                            Temperature = 22.3m,
                            _dataCreated = new DateTime(2020, 2, 13, 10, 18, 55, 388, DateTimeKind.Utc).AddTicks(7947)
                        },
                        new
                        {
                            Id = 2,
                            CoinsCurrentSupply = 0m,
                            CoinsInMachine = 15.55m,
                            LatestCleaningMachine = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LatestLoadedProducts = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LatestMoneyCollection = new DateTimeOffset(new DateTime(2020, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineTypeId = 2,
                            Position = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (10.9839598 45.4518442)"),
                            Status = true,
                            Temperature = 22.3m,
                            _dataCreated = new DateTime(2020, 2, 13, 10, 18, 55, 389, DateTimeKind.Utc).AddTicks(6816)
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.MachineType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Model", "Version")
                        .IsUnique()
                        .HasName("IX_UniqueMachineVersion")
                        .HasFilter("[Model] IS NOT NULL");

                    b.ToTable("MachineTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Model = "BVM",
                            Version = "Coffee"
                        },
                        new
                        {
                            Id = 2,
                            Model = "MiniSnakky",
                            Version = "FrigoAndCoffee"
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ActivationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MachineItemId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_ActiveProducts");

                    b.HasIndex("MachineItemId");

                    b.ToTable("ActiveProducts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 2,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 3,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 6,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        },
                        new
                        {
                            Id = 7,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        },
                        new
                        {
                            Id = 8,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.ProductConsumed", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ActivationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MachineItemId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ProvidedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("MachineItemId");

                    b.ToTable("HistoryProducts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 2,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 3,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1
                        },
                        new
                        {
                            Id = 4,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1,
                            ProvidedDate = new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0))
                        },
                        new
                        {
                            Id = 5,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 1,
                            ProvidedDate = new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0))
                        },
                        new
                        {
                            Id = 6,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        },
                        new
                        {
                            Id = 7,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        },
                        new
                        {
                            Id = 8,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2
                        },
                        new
                        {
                            Id = 9,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2,
                            ProvidedDate = new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0))
                        },
                        new
                        {
                            Id = 10,
                            ActivationDate = new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            MachineItemId = 2,
                            ProvidedDate = new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.MachineItem", b =>
                {
                    b.HasOne("VendingMachine.Service.Machines.Domain.MachineType", "MachineType")
                        .WithMany()
                        .HasForeignKey("MachineTypeId");
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.Product", b =>
                {
                    b.HasOne("VendingMachine.Service.Machines.Domain.MachineItem", null)
                        .WithMany("ActiveProducts")
                        .HasForeignKey("MachineItemId");
                });

            modelBuilder.Entity("VendingMachine.Service.Machines.Domain.ProductConsumed", b =>
                {
                    b.HasOne("VendingMachine.Service.Machines.Domain.MachineItem", null)
                        .WithMany("HistoryProducts")
                        .HasForeignKey("MachineItemId");
                });
#pragma warning restore 612, 618
        }
    }
}
