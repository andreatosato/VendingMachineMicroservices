using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using VendingMachine.Service.Machines.Data.Seeds;

namespace VendingMachine.Service.Machines.Data.Migrations
{
    public partial class FirstMigrationMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(maxLength: 255, nullable: true),
                    Version = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<Point>(nullable: true),
                    Temperature = table.Column<decimal>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    MachineTypeId = table.Column<int>(nullable: true),
                    LatestLoadedProducts = table.Column<DateTimeOffset>(nullable: true),
                    LatestCleaningMachine = table.Column<DateTimeOffset>(nullable: true),
                    LatestMoneyCollection = table.Column<DateTimeOffset>(nullable: true),
                    CoinsInMachine = table.Column<decimal>(nullable: false),
                    CoinsCurrentSupply = table.Column<decimal>(nullable: false),
                    DataCreated = table.Column<DateTime>(nullable: false),
                    DataUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_MachineTypes_MachineTypeId",
                        column: x => x.MachineTypeId,
                        principalTable: "MachineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActiveProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Machines_MachineTypeId",
                table: "Machines",
                column: "MachineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueMachineVersion",
                table: "MachineTypes",
                columns: new[] { "Model", "Version" },
                unique: true,
                filter: "[Model] IS NOT NULL");



            migrationBuilder._1();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveProducts");

            migrationBuilder.DropTable(
                name: "HistoryProducts");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "MachineTypes");
        }
    }
}
