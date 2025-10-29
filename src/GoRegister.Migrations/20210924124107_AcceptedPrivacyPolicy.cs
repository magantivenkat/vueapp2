using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AcceptedPrivacyPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptedPrivacyPolicy",
                table: "DelegateUser",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedPrivacyPolicyDateUtc",
                table: "DelegateUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedPrivacyPolicy",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "AcceptedPrivacyPolicyDateUtc",
                table: "DelegateUser");
        }
    }
}
