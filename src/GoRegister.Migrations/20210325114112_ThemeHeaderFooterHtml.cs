using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ThemeHeaderFooterHtml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterHtml",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderHtml",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEL0ub5W8qkTuAFZIIqO9Igj5zBJitBqjV5Ph5DAgYSNNtmxQyigeBgKM4XZzk05rmA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterHtml",
                table: "ProjectTheme");

            migrationBuilder.DropColumn(
                name: "HeaderHtml",
                table: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPTUpmgx3yhtxaGoGssMOo1OG9eB7Eeoot3h7A4UEdNyiC3tS4ZLRTZUkk7mAFmuFA==");
        }
    }
}
