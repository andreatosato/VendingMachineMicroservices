﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VendingMachine.Service.Products.Data;

namespace VendingMachine.Service.Products.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Version")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ProductEntity");
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.ProductItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("Purchased")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("Sold")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ExpirationDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProductId = 3,
                            Purchased = new DateTimeOffset(new DateTime(2020, 1, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Sold = new DateTimeOffset(new DateTime(2020, 1, 6, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.ColdDrinkEntity", b =>
                {
                    b.HasBaseType("VendingMachine.Service.Products.Data.Entities.ProductEntity");

                    b.Property<decimal>("Litre")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TemperatureMaximum")
                        .HasColumnName("TemperatureMaximum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TemperatureMinimum")
                        .HasColumnName("TemperatureMinimum")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("ColdDrinkEntity");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Acqua",
                            Litre = 0.5m,
                            TemperatureMaximum = 35m,
                            TemperatureMinimum = 0m
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.HotDrinkEntity", b =>
                {
                    b.HasBaseType("VendingMachine.Service.Products.Data.Entities.ProductEntity");

                    b.Property<decimal>("Grams")
                        .HasColumnName("Grams")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TemperatureMaximum")
                        .HasColumnName("TemperatureMaximum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TemperatureMinimum")
                        .HasColumnName("TemperatureMinimum")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("HotDrinkEntity");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "Caffè",
                            Grams = 7m,
                            TemperatureMaximum = 25m,
                            TemperatureMinimum = 15m
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.SnackEntity", b =>
                {
                    b.HasBaseType("VendingMachine.Service.Products.Data.Entities.ProductEntity");

                    b.Property<decimal>("Grams")
                        .HasColumnName("Grams")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("SnackEntity");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            Name = "Kinder Delice",
                            Grams = 40m
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.ProductEntity", b =>
                {
                    b.OwnsOne("VendingMachine.Service.Products.Data.Entities.GrossPriceEntity", "Price", b1 =>
                        {
                            b1.Property<int>("ProductEntityId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("GrossPrice")
                                .HasColumnName("GrossPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal>("NetPrice")
                                .HasColumnName("NetPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal>("Rate")
                                .HasColumnName("Rate")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int>("TaxPercentage")
                                .HasColumnName("TaxPercentage")
                                .HasColumnType("int");

                            b1.HasKey("ProductEntityId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductEntityId");

                            b1.HasData(
                                new
                                {
                                    ProductEntityId = 1,
                                    GrossPrice = 0.50m,
                                    NetPrice = 0.48m,
                                    Rate = 0.02m,
                                    TaxPercentage = 4
                                },
                                new
                                {
                                    ProductEntityId = 2,
                                    GrossPrice = 0.50m,
                                    NetPrice = 0.48m,
                                    Rate = 0.02m,
                                    TaxPercentage = 4
                                },
                                new
                                {
                                    ProductEntityId = 3,
                                    GrossPrice = 0.70m,
                                    NetPrice = 0.67m,
                                    Rate = 0.03m,
                                    TaxPercentage = 4
                                });
                        });
                });

            modelBuilder.Entity("VendingMachine.Service.Products.Data.Entities.ProductItemEntity", b =>
                {
                    b.HasOne("VendingMachine.Service.Products.Data.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.OwnsOne("VendingMachine.Service.Products.Data.Entities.GrossPriceEntity", "SoldPrice", b1 =>
                        {
                            b1.Property<int>("ProductItemEntityId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("GrossPrice")
                                .HasColumnName("GrossPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal>("NetPrice")
                                .HasColumnName("NetPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal>("Rate")
                                .HasColumnName("Rate")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int>("TaxPercentage")
                                .HasColumnName("TaxPercentage")
                                .HasColumnType("int");

                            b1.HasKey("ProductItemEntityId");

                            b1.ToTable("ProductItems");

                            b1.WithOwner()
                                .HasForeignKey("ProductItemEntityId");

                            b1.HasData(
                                new
                                {
                                    ProductItemEntityId = 1,
                                    GrossPrice = 0.90m,
                                    NetPrice = 0.86m,
                                    Rate = 0.04m,
                                    TaxPercentage = 4
                                });
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
