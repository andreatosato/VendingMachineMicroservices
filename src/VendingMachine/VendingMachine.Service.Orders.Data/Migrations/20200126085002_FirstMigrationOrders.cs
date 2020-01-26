using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Orders.Data.Migrations
{
    public partial class FirstMigrationOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(nullable: true),
                    CoinsCurrentSupply = table.Column<decimal>(nullable: true),
                    OrderDate = table.Column<DateTimeOffset>(nullable: false),
                    Processed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProductItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductItemId = table.Column<int>(nullable: false),
                    GrossPrice = table.Column<decimal>(nullable: true),
                    NetPrice = table.Column<decimal>(nullable: true),
                    TaxPercentage = table.Column<int>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProductItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductItem_OrderId",
                table: "OrderProductItem",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProductItem");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
