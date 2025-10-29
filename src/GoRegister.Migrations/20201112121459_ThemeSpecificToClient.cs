using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ThemeSpecificToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEGuMyTJpxXhn4SryeiCYcJLKdKrRukKUr+c20yKnFN7uRCXTqxpzZLXtjoeVAmpORg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEC6LEzY71UIPa+JAGqRyELxK5LgVkwx3TAFbxMrzcwk/KY7OoPZ+Y4hHE9t2D0w7Tg==");
        }
    }
}
