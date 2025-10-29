using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DeclineRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDecline",
                table: "RegistrationPath",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeclineTo",
                table: "RegistrationPath",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeclineDocument",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAENTqaCsPfY+yzMM9JpOlju4NPEc4xxsKjOAx03ALXyTmKOJGnBYTovXbQdaKH45eXQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDecline",
                table: "RegistrationPath");

            migrationBuilder.DropColumn(
                name: "DateDeclineTo",
                table: "RegistrationPath");

            migrationBuilder.DropColumn(
                name: "DeclineDocument",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEBqzEbpcdeDB/HdCH23ZlxwgfrYnaeEqASFghmbo7o3hQvo75lbbWpjrcNeQ/uN7Lg==");
        }
    }
}
