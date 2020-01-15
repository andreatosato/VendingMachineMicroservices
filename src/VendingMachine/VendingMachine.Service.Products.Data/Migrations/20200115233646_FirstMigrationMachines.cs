using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Products.Data.Migrations
{
    public partial class FirstMigrationMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrossPrice = table.Column<decimal>(nullable: true),
                    NetPrice = table.Column<decimal>(nullable: true),
                    TaxPercentage = table.Column<int>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<byte[]>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    TemperatureMinimum = table.Column<decimal>(nullable: true),
                    TemperatureMax = table.Column<decimal>(nullable: true),
                    Litre = table.Column<decimal>(nullable: true),
                    Grams = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: true),
                    Purchased = table.Column<DateTimeOffset>(nullable: false),
                    Sold = table.Column<DateTimeOffset>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    GrossPrice = table.Column<decimal>(nullable: true),
                    NetPrice = table.Column<decimal>(nullable: true),
                    TaxPercentage = table.Column<int>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Discriminator", "Name", "Version", "Litre", "TemperatureMax", "TemperatureMinimum", "NetPrice", "Rate", "TaxPercentage", "GrossPrice" },
                values: new object[] { 1, "ColdDrinkEntity", "Acqua", null, 0.5m, 35m, 0m, 0.48m, 0.02m, 4, 0.50m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Discriminator", "Name", "Version", "Grams", "TemperatureMax", "TemperatureMinimum", "NetPrice", "Rate", "TaxPercentage", "GrossPrice" },
                values: new object[] { 2, "HotDrinkEntity", "Caffè", null, 7m, 25m, 15m, 0.48m, 0.02m, 4, 0.50m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Discriminator", "Name", "Version", "Grams", "NetPrice", "Rate", "TaxPercentage", "GrossPrice" },
                values: new object[] { 3, "SnakEntity", "Kinder Delice", null, 40m, 0.67m, 0.03m, 4, 0.70m });

            migrationBuilder.InsertData(
                table: "ProductItems",
                columns: new[] { "Id", "ExpirationDate", "ProductId", "Purchased", "Sold", "NetPrice", "Rate", "TaxPercentage", "GrossPrice" },
                values: new object[] { 1, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTimeOffset(new DateTime(2020, 1, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 6, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0.86m, 0.04m, 4, 0.90m });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ProductId",
                table: "ProductItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
