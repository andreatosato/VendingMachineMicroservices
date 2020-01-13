using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Authentications.API.Migrations
{
    public partial class SecondMigrationMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("facbd4f2-da0f-4ef9-afeb-770d08ff7c2f"), "aa6dbeda-9fa6-480e-8288-a243b3a6d043", "User", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("d5398c47-f617-4dc6-885a-32bdbe43877a"), "a7ef2388-3da8-497d-97a8-ae99f8fd4e04", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("9c346aeb-22d7-4da4-ada4-88d8ab67978e"), 0, "7d61c568-91ee-4667-93a2-65626c84aeaf", "andrea.tosato@4ward.it", true, "Andrea", "Tosato", false, null, "andrea.tosato@4ward.it", "andrea.tosato@4ward.it", "AQAAAAEAACcQAAAAEFXdckTvNYlv2ztV/cC6u0fJsnLcadlMXaW94rQ/WgXC82aNA4mIYMjFKBxm3GWZOQ==", null, false, null, false, "andrea.tosato@4ward.it" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("9c346aeb-22d7-4da4-ada4-88d8ab67978e"), new Guid("d5398c47-f617-4dc6-885a-32bdbe43877a") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("facbd4f2-da0f-4ef9-afeb-770d08ff7c2f"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("9c346aeb-22d7-4da4-ada4-88d8ab67978e"), new Guid("d5398c47-f617-4dc6-885a-32bdbe43877a") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d5398c47-f617-4dc6-885a-32bdbe43877a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9c346aeb-22d7-4da4-ada4-88d8ab67978e"));
        }
    }
}
