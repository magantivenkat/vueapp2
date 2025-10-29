using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTypeToSingleSelectField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SingleSelectType",
                table: "Field",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "492417fd-8ce1-4f6e-a7b8-83da3f9bf574");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "238b73cb-14d2-4680-a611-2658bc17ca3b", "AQAAAAEAACcQAAAAEGSv12AJTYsieKWZtQkF1irDMQ8hCM7U/3035V6fsdjHQC7XipBOB/OxIQgQHd0tGA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SingleSelectType",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "83e37a33-490c-433d-a444-57389da35095");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d52786df-d8ab-4dad-a5c1-8db0b9402b58", "AQAAAAEAACcQAAAAEOLK43e6Nh0couSzA1884+EIoodNbLAynegkvkPdkEBO3zMysWDW7lpQf4DjmsiQUA==" });
        }
    }
}
