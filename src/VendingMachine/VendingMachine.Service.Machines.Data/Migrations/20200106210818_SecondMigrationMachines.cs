using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Machines.Data.Migrations
{
    public partial class SecondMigrationMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoinsCurrentSupply",
                table: "Machines",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinsCurrentSupply",
                table: "Machines");
        }
    }
}
