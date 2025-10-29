using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddHelpTextToField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HelpTextAfter",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HelpTextBefore",
                table: "Field",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HelpTextAfter",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "HelpTextBefore",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ae6d23bd-3e34-43eb-aabc-f3963b993149");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4a0f598a-0229-4604-8e6d-35318fd4b9a6", "AQAAAAEAACcQAAAAEOG5R0nWXof19wg9dSfDxPAFPtkTAJvtcH+AoDIwuDha596DQcJfOsM4dlIykM/rcA==" });
        }
    }
}
