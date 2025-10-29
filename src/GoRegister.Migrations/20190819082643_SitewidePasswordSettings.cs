using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SitewidePasswordSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSitewidePasswordEnabled",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SitewidePasswordHashed",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SitewidePasswordPlainText",
                table: "Project",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ea27adf7-1067-4b75-af3a-05358e4518bc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b96c0d11-40a5-482f-9746-10e69d610736", "AQAAAAEAACcQAAAAEMNLkDObcyEmD8NLvbtsaeXbYGwe88gKlb2BPe6Z6bbtVAIN9yvG1KGvKZk1cZsVdQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSitewidePasswordEnabled",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "SitewidePasswordHashed",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "SitewidePasswordPlainText",
                table: "Project");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a403dcb4-4121-4595-af1f-1690ae2f52b1");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f5142003-cc7a-4535-ba2d-3cc7c959606a", "AQAAAAEAACcQAAAAEGgOjySpZGo7N8+ibhY2qBAk4F46mRTJbrigU9PuNxwVmmLRUZ4QqxUdnPPR8wA9MQ==" });
        }
    }
}
