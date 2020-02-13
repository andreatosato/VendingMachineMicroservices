using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VendingMachine.Service.Authentications.API.Migrations
{
    public partial class SecondMigrationAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("844e9e57-9282-4779-8b3a-eb87b9f9add1"));

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7"), new Guid("5f4f86b0-c668-4919-9725-0c8875a8736f") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5f4f86b0-c668-4919-9725-0c8875a8736f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("f4b49977-b119-4764-a317-b321dc80be7c"), "b6f7c68e-5f24-4967-83fe-bc5eea69fb1e", "User", null },
                    { new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4"), "1ff59ed3-50a8-49e2-b0b2-d4e508d96b6f", "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"), 0, "ace209b6-940a-472a-8326-20328d991df8", "andrea.tosato@4ward.it", true, "Andrea", "Tosato", false, null, "andrea.tosato@4ward.it", "andrea.tosato@4ward.it", "AQAAAAEAACcQAAAAEBZvFdXTRYmqm6Bz4j5rUCQITBE+NC1pPrd1+39aFprFdOxcldCDG9jFuQi0jxZGsQ==", null, false, null, false, "andrea.tosato@4ward.it" },
                    { new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"), 0, "d9f54081-5a89-4706-9703-dde7d5295a0e", "test@cloudgen.it", true, "Associazione", "Cloudgen", false, null, "test@cloudgen.it", "test@cloudgen.it", "AQAAAAEAACcQAAAAEM3qFS6FxNJF9cKePibhEw22rUPra26LqDTSPP4i/MOFKXHvEWIrQD7xrsBs/98jIw==", null, false, null, false, "test@cloudgen.it" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 3, "VendingMachineApiClaim", "Order.Api", new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319") },
                    { 2, "VendingMachineApiClaim", "Product.Api", new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319") },
                    { 1, "VendingMachineApiClaim", "Machine.Api", new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319") },
                    { 4, "VendingMachineApiClaim", "Machine.Api", new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a") },
                    { 5, "VendingMachineApiClaim", "Product.Api", new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a") },
                    { 6, "VendingMachineApiClaim", "Order.Api", new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"), new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4") },
                    { new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"), new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4") },
                    { new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"), new Guid("f4b49977-b119-4764-a317-b321dc80be7c") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"), new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"), new Guid("f4b49977-b119-4764-a317-b321dc80be7c") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"), new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f4b49977-b119-4764-a317-b321dc80be7c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fb0cbd71-a135-4d59-a859-2a4ed81834f4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("04e417d3-fcb9-4494-8ef5-4c863d82d319"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7e9dec2b-1c41-4285-82e0-b081f49cff9a"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("844e9e57-9282-4779-8b3a-eb87b9f9add1"), "4e0666e3-f79e-4ca8-8509-2235bd3578b6", "User", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5f4f86b0-c668-4919-9725-0c8875a8736f"), "e9324026-7d28-46be-82ff-debc16f65b5a", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7"), 0, "cb8566f5-3fe4-4b14-9adb-e628fcc1f80c", "andrea.tosato@4ward.it", true, "Andrea", "Tosato", false, null, "andrea.tosato@4ward.it", "andrea.tosato@4ward.it", "AQAAAAEAACcQAAAAEAMm7dx7OYx9kQZkMJ9UEUVqlqZ+OntwYtYCzX+Gu4w4n0ojXmi09OX+Vwi6XK/bQw==", null, false, null, false, "andrea.tosato@4ward.it" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 3, "VendingMachineApiClaim", "Order.Api", new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7") },
                    { 2, "VendingMachineApiClaim", "Product.Api", new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7") },
                    { 1, "VendingMachineApiClaim", "Machine.Api", new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("aa172fc2-786d-4f47-bb30-32b77bc3f3e7"), new Guid("5f4f86b0-c668-4919-9725-0c8875a8736f") });
        }
    }
}
