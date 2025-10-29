using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AllowAnonymousProjectSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowAnonymousAccess",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEKeYk0ig9VV4OyCj0FNYDu4LJUG+x6n0iCpRFD0dYpf6yrm/lTLRe257UjdaGCylmg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAnonymousAccess",
                table: "Project");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFgk8PbTLtuqmVoNmbCjWCMjWfdGqUiFvQU3WKAUp6uo2LSU+0tCZpXaTPRdXBHJtA==");
        }
    }
}
