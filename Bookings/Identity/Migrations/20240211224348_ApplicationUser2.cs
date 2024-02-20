using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookings.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1aa83d5e-856d-48ba-bf38-4e979b4a2298");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b25cdfa3-a0ea-493c-84fa-836d5ba4768a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5edc60da-9d28-4e34-979c-1c1c77046d8f", null, "Admin", "ADMIN" },
                    { "bea62b02-02c9-4e56-a77c-64715968d423", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5edc60da-9d28-4e34-979c-1c1c77046d8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bea62b02-02c9-4e56-a77c-64715968d423");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1aa83d5e-856d-48ba-bf38-4e979b4a2298", null, "User", "USER" },
                    { "b25cdfa3-a0ea-493c-84fa-836d5ba4768a", null, "Admin", "ADMIN" }
                });
        }
    }
}
