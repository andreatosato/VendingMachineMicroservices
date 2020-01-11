using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Authentications.API.Migrations
{
    public partial class SecondMigrationAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("a4f066b6-833d-4c6f-bed4-77cbf83bda1c"), "d92d527f-0ca4-40f5-9dad-6f7ae92172d1", "User", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("0e79c05e-b046-43ad-b255-7a5e015f345c"), "4a51f3ab-b7cf-451f-8891-5983a1d572ca", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("96c8fd76-fff3-4aa0-afec-679cbf01f497"), 0, "90bedd48-5a12-4e81-bc39-1abeee8397a7", "andrea.tosato@4ward.it", true, "Andrea", "Tosato", false, null, null, null, "AQAAAAEAACcQAAAAEE0Q7h/ZxkG7DzE4E7NQZyYnE0Et8+5ee09LOSxtg7LzPJ7U1WilnOJGfY2ShWbbFA==", null, false, null, false, "andrea.tosato@4ward.it" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("96c8fd76-fff3-4aa0-afec-679cbf01f497"), new Guid("0e79c05e-b046-43ad-b255-7a5e015f345c") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a4f066b6-833d-4c6f-bed4-77cbf83bda1c"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("96c8fd76-fff3-4aa0-afec-679cbf01f497"), new Guid("0e79c05e-b046-43ad-b255-7a5e015f345c") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0e79c05e-b046-43ad-b255-7a5e015f345c"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("96c8fd76-fff3-4aa0-afec-679cbf01f497"));
        }
    }
}
