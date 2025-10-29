using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ThemeLogoUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "ProjectTheme",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThemeUniqueId",
                table: "ProjectTheme",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFiyWMHYIKjofV9l71xJYnjwsH6XK4grXuNQOcVKKxEl1pP9ZAv94MCQNZhuLsISxQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "ProjectTheme");

            migrationBuilder.DropColumn(
                name: "ThemeUniqueId",
                table: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDNCGcc8SSjjhQC/nK5hkbySIdz0Nih+L1LhEsu5nA4dSa/eGjhcpOgZJLQOtFYRGg==");
        }
    }
}
