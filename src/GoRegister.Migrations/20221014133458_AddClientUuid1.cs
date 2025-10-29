using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddClientUuid1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MRFClientRequestCountry_MRFClientRequest_MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry");

            //migrationBuilder.DropIndex(
            //    name: "IX_MRFClientRequestCountry_MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry");

            //migrationBuilder.DropColumn(
            //    name: "MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry");

            //migrationBuilder.AddColumn<string>(
            //    name: "ClientUuid",
            //    table: "Client",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MRFClientRequestCountry_ClientuniqueID",
            //    table: "MRFClientRequestCountry",
            //    column: "ClientuniqueID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MRFClientRequestCountry_MRFClientRequest_ClientuniqueID",
            //    table: "MRFClientRequestCountry",
            //    column: "ClientuniqueID",
            //    principalTable: "MRFClientRequest",
            //    principalColumn: "ClientuniqueID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MRFClientRequestCountry_MRFClientRequest_ClientuniqueID",
            //    table: "MRFClientRequestCountry");

            //migrationBuilder.DropIndex(
            //    name: "IX_MRFClientRequestCountry_ClientuniqueID",
            //    table: "MRFClientRequestCountry");

            //migrationBuilder.DropColumn(
            //    name: "ClientUuid",
            //    table: "Client");

            //migrationBuilder.AddColumn<int>(
            //    name: "MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MRFClientRequestCountry_MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry",
            //    column: "MRFClientRequestClientuniqueID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MRFClientRequestCountry_MRFClientRequest_MRFClientRequestClientuniqueID",
            //    table: "MRFClientRequestCountry",
            //    column: "MRFClientRequestClientuniqueID",
            //    principalTable: "MRFClientRequest",
            //    principalColumn: "ClientuniqueID",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
