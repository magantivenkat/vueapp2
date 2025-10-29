using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddIsVisibleColumnToRegistrationPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "RegistrationPage",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "78bf2202-bf6b-4361-be8c-fb450f0796be");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c551f914-69a0-4ccc-a82d-1b99a36547e8", "AQAAAAEAACcQAAAAEM4rzyxJTku+AaPxAXxzS+WSmts/Q82B9FKMyO3YUXbYdNnfKy5CyB8mNeylMiiGOg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "RegistrationPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "663708a8-a256-4cc4-aa04-f2f39e0feae6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1fbca0a0-403e-47b3-834d-6d45b8a5236d", "AQAAAAEAACcQAAAAEKnBl8mIaPiD0WCyPOzHXOsUf3ylkFx9WueT5t6+fac+sL0V3CYsB+BvwBntDlv06g==" });
        }
    }
}
