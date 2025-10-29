using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateSessionBookingsReleaseDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReleasedUtc",
                table: "DelegateSessionBooking",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDqRSOPy7lJFhCUo0OGSTWSq8kPFMs86BT4lSRleOYupqt6gjjxxBl9yT9d1/XW4xg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReleasedUtc",
                table: "DelegateSessionBooking");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIfe3RAdKdwvcFfwyzof5EBXQm1kfEWoIE6R/h7klaMe3FK5ulD4O6u6HoGtrwRP2w==");
        }
    }
}
