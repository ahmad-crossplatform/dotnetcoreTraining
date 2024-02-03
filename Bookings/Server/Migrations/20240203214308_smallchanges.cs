using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Server.Migrations
{
    /// <inheritdoc />
    public partial class smallchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_User_UserEmail",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserEmail",
                table: "Bookings",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserEmail",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_User_UserEmail",
                table: "Bookings",
                column: "UserEmail",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
