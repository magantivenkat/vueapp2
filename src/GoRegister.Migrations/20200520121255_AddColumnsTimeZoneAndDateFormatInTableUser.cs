using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddColumnsTimeZoneAndDateFormatInTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "User",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b62b5c80-6656-4048-b148-88ec07a89976");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a048f23d-ad3d-4e6b-a5ff-5f439d24001c", "AQAAAAEAACcQAAAAEJd5Mr/buQuDM+cn1h08AAgCRcfI8WNkO3utSD0q4qFy7J+gMpshg5NQRe6shW1+Rg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFormat",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "User");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "23005f8b-93ad-41fa-8054-212597b35a0c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b5842d3b-520b-4e08-aef7-d327c123282d", "AQAAAAEAACcQAAAAEHIufU2WqBtjNu8vWGmN9Cu4S71KnQlkieZKFV+Cg0DmH91hjuRSzOEVZ1uGSSF0Tg==" });
        }
    }
}
