using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class TPNCountryClientEmailwithUuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TPNCountryClientEmail_Client_ClientId",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_TPNCountryClientEmail_Country_CountriesISO",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropIndex(
                name: "IX_TPNCountryClientEmail_CountriesISO",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropColumn(
                name: "CountriesISO",
                table: "TPNCountryClientEmail");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "TPNCountryClientEmail",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientUuid",
                table: "TPNCountryClientEmail",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TPNCountryClientEmailId",
                table: "Country",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_TPNCountryClientEmailId",
                table: "Country",
                column: "TPNCountryClientEmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_TPNCountryClientEmail_TPNCountryClientEmailId",
                table: "Country",
                column: "TPNCountryClientEmailId",
                principalTable: "TPNCountryClientEmail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TPNCountryClientEmail_Client_ClientId",
                table: "TPNCountryClientEmail",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_TPNCountryClientEmail_TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_TPNCountryClientEmail_Client_ClientId",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropIndex(
                name: "IX_Country_TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "ClientUuid",
                table: "TPNCountryClientEmail");

            migrationBuilder.DropColumn(
                name: "TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "TPNCountryClientEmail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CountriesISO",
                table: "TPNCountryClientEmail",
                type: "nvarchar(2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TPNCountryClientEmail_CountriesISO",
                table: "TPNCountryClientEmail",
                column: "CountriesISO");

            migrationBuilder.AddForeignKey(
                name: "FK_TPNCountryClientEmail_Client_ClientId",
                table: "TPNCountryClientEmail",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TPNCountryClientEmail_Country_CountriesISO",
                table: "TPNCountryClientEmail",
                column: "CountriesISO",
                principalTable: "Country",
                principalColumn: "ISO",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
