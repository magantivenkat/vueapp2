using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DefaultHomepageHtml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomepageHtml",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIuEBKB/4dhxVEBVdT/NMVeHI9bFvnimeyH1pro1ZpiWy36nbhB7r6bhiSOlvnP25w==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomepageHtml",
                table: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIAPS4uO+r4ICCBMmTKwm2PEi4/he6ce8qpJ133YRRkZ/9euKrKxAdhTTZCJ3N2Vaw==");
        }
    }
}
