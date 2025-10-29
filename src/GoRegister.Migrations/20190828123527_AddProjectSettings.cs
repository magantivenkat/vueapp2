using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddProjectSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveDate",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDataDate",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailDisplayFrom",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailReplyTo",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExternalReference",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "Project",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2f58661b-8d06-44ec-9a62-cdb0971d2399");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "213294c5-4051-49b4-a927-5af2aedabe86", "AQAAAAEAACcQAAAAEDOEwKGNv/GgdFeL6ZalKL5JFn2BYCSY2467rhffD1Y4X/2KFt2n8n2N2osF75KPjQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchiveDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DeleteDataDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EmailDisplayFrom",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EmailReplyTo",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ExternalReference",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "Project");

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
    }
}
