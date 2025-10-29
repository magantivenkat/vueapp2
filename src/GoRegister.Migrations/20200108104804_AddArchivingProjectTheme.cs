using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddArchivingProjectTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateArchived",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "ProjectTheme",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f0a144a3-7eed-4977-8bc4-15119ac218eb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "45966104-c6fe-4efb-9a00-654e1101b3e9", "AQAAAAEAACcQAAAAEIXIY1ac+N0ePXjJXDMcQhxg5uhNh1eWRiKZYe3WZqdXjWbNURGsDV2946rZFAbfMA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateArchived",
                table: "ProjectTheme");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "ProjectTheme");

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
    }
}
