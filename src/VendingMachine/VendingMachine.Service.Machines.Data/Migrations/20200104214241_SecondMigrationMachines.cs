using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Machines.Data.Migrations
{
    public partial class SecondMigrationMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoinsInMachine",
                table: "Machines",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LatestCleaningMachine",
                table: "Machines",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LatestLoadedProducts",
                table: "Machines",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LatestMoneyCollection",
                table: "Machines",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActiveProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationDate = table.Column<DateTimeOffset>(nullable: true),
                    MachineItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveProducts_Machines_MachineItemId",
                        column: x => x.MachineItemId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvidedDate = table.Column<DateTimeOffset>(nullable: true),
                    ActivationDate = table.Column<DateTimeOffset>(nullable: true),
                    MachineItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryProducts_Machines_MachineItemId",
                        column: x => x.MachineItemId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveProducts_MachineItemId",
                table: "ActiveProducts",
                column: "MachineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryProducts_MachineItemId",
                table: "HistoryProducts",
                column: "MachineItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveProducts");

            migrationBuilder.DropTable(
                name: "HistoryProducts");

            migrationBuilder.DropColumn(
                name: "CoinsInMachine",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LatestCleaningMachine",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LatestLoadedProducts",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "LatestMoneyCollection",
                table: "Machines");
        }
    }
}
