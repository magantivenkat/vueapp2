using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SessionNotesAndDelegateBookingAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCloseRegistration",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "Session");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCloseRegistrationUtc",
                table: "Session",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreatedUtc",
                table: "Session",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEndUtc",
                table: "Session",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStartUtc",
                table: "Session",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Session",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionedByUserId",
                table: "DelegateSessionBooking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateActionedUtc",
                table: "DelegateSessionBooking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHwJeYBXLgKZW0bSyby4QLL0M6cFUTCWG8IOhlyEz5ZN0KaMlf4fycRYyul5vFCsNA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCloseRegistrationUtc",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateCreatedUtc",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateEndUtc",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "DateStartUtc",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "ActionedByUserId",
                table: "DelegateSessionBooking");

            migrationBuilder.DropColumn(
                name: "DateActionedUtc",
                table: "DelegateSessionBooking");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCloseRegistration",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK6gRjSaPvFL2BvK7AsmM02V7v//FoYlBgqV1R4eznO91EesFkAZ8PI6azoG7dNkSw==");
        }
    }
}
