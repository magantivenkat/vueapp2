using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveUnusedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgendaId",
                table: "FieldOption");

            migrationBuilder.DropColumn(
                name: "VisaAllowance",
                table: "FieldOption");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "DeclinedDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "InvitedDate",
                table: "DelegateUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "FieldOption",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFgk8PbTLtuqmVoNmbCjWCMjWfdGqUiFvQU3WKAUp6uo2LSU+0tCZpXaTPRdXBHJtA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "FieldOption");

            migrationBuilder.AddColumn<int>(
                name: "AgendaId",
                table: "FieldOption",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VisaAllowance",
                table: "FieldOption",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "DelegateUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "DelegateUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedDate",
                table: "DelegateUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitedDate",
                table: "DelegateUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMcdei2X5jIM3FEhQNvsBL6m2KLfYtipWFSHJBsWCqInbIgGbuIQ/J695VeWSVTwAQ==");
        }
    }
}
