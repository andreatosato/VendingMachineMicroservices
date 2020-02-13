using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

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

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Model", "Version" },
                values: new object[] { 1, "BVM", "Coffee" });

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Model", "Version" },
                values: new object[] { 2, "MiniSnakky", "FrigoAndCoffee" });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "CoinsCurrentSupply", "CoinsInMachine", "LatestCleaningMachine", "LatestLoadedProducts", "LatestMoneyCollection", "MachineTypeId", "Position", "Status", "Temperature", "DataCreated", "DataUpdated" },
                values: new object[] { 1, 0m, 15.55m, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (10.9928686 45.4041261)"), true, 22.3m, new DateTime(2020, 2, 13, 10, 18, 55, 388, DateTimeKind.Utc).AddTicks(7947), null });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "CoinsCurrentSupply", "CoinsInMachine", "LatestCleaningMachine", "LatestLoadedProducts", "LatestMoneyCollection", "MachineTypeId", "Position", "Status", "Temperature", "DataCreated", "DataUpdated" },
                values: new object[] { 2, 0m, 15.55m, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (10.9839598 45.4518442)"), true, 22.3m, new DateTime(2020, 2, 13, 10, 18, 55, 389, DateTimeKind.Utc).AddTicks(6816), null });

            migrationBuilder.InsertData(
                table: "ActiveProducts",
                columns: new[] { "Id", "ActivationDate", "MachineItemId" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 2, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 3, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 6, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2 },
                    { 7, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2 },
                    { 8, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2 }
                });

            migrationBuilder.InsertData(
                table: "HistoryProducts",
                columns: new[] { "Id", "ActivationDate", "MachineItemId", "ProvidedDate" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null },
                    { 2, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null },
                    { 3, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null },
                    { 4, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 5, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 6, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null },
                    { 7, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null },
                    { 8, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null },
                    { 9, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 10, new DateTimeOffset(new DateTime(2020, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, new DateTimeOffset(new DateTime(2020, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
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
