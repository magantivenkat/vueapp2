using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddThemeGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ThemeGuid",
                table: "ProjectTheme",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "dc921d9e-598a-4479-8b37-d3ad525a6246");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4ce4ffc3-eddc-4926-891d-40dd61b1b2be", "AQAAAAEAACcQAAAAEDdHdC8iY82uekbsP8En3pqXlq0qrLsIUbTNt31OGjwkOEiEYtsX7cbROpS1DIH+KA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeGuid",
                table: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d1a20240-06af-4df6-aef7-fd8780561e12");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "afc335c3-6c4c-494f-9f93-86c2514aae69", "AQAAAAEAACcQAAAAEMzU83vWYJJmvUKZKoN/4O9mzQySqG7bKVgqWMmBllg+TVPlsXSPTc641lnjnc+eOA==" });
        }
    }
}
