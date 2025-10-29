using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTPNColsMRFClientResponseDetails_20072023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowTPNCountries",
                table: "MRFClientResponseDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendTPNEmailDateTime",
                table: "MRFClientResponseDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowTPNCountries",
                table: "MRFClientResponseDetails");

            migrationBuilder.DropColumn(
                name: "SendTPNEmailDateTime",
                table: "MRFClientResponseDetails");
        }
    }
}
