using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "isAuthenticated",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "otp",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Branch", "Email", "FullName", "LoginId", "Nin", "PhoneNumber", "ProfilePicture", "Role", "Sex", "isAuthenticated", "otp" },
                values: new object[] { 1, "Admin HQ", "Main Branch", "md.erhenede@gmail.com", "Super Admin", "admin001", "ADMIN123NIN", "09034325561", "default.png", "Admin", "Other", false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "isAuthenticated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "otp",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
