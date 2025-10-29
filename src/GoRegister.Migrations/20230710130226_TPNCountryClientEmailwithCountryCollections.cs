using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class TPNCountryClientEmailwithCountryCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountriesISO",
                table: "TPNCountryClientEmail",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TPNCountryClientEmail_CountriesISO",
                table: "TPNCountryClientEmail",
                column: "CountriesISO");

            migrationBuilder.AddForeignKey(
                name: "FK_TPNCountryClientEmail_Country_CountriesISO",
                table: "TPNCountryClientEmail",
                column: "CountriesISO",
                principalTable: "Country",
                principalColumn: "ISO",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TPNCountryClientEmail_Country_CountriesISO",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropIndex(
                name: "IX_TPNCountryClientEmail_CountriesISO",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropColumn(
                name: "CountriesISO",
                table: "TPNCountryClientEmail");
        }
    }
}
