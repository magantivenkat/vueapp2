using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTPNReportTPNRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_TPNCountryClientEmail_TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "TPNCountryClientEmailId",
                table: "Country");

            migrationBuilder.CreateTable(
                name: "TPNReportDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRFResponseDetailsId = table.Column<int>(nullable: true),
                    MRFFormName = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: true),
                    ClientUuid = table.Column<string>(nullable: true),
                    GBTClient = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    TPNCountry = table.Column<string>(nullable: true),
                    TPNSharedMailbox = table.Column<string>(nullable: true),
                    ContactFirstName = table.Column<string>(nullable: true),
                    ContactLastName = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    DepartureDate = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    EventName = table.Column<string>(nullable: true),
                    FormDateTimeCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPNReportDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TPNReportRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    ClientUuid = table.Column<string>(nullable: true),
                    TPNCountry = table.Column<string>(nullable: true),
                    RequestedDate = table.Column<DateTime>(nullable: false),
                    RequestedBy = table.Column<int>(nullable: false),
                    DownloadPath = table.Column<string>(nullable: true),
                    ReportType = table.Column<string>(nullable: true),
                    ReportStatusId = table.Column<int>(nullable: false),
                    GAMEmail = table.Column<string>(nullable: true),
                    ReportFrequency = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPNReportRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPNReportRequest_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TPNReportRequest_ClientId",
                table: "TPNReportRequest",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPNReportDetails");

            migrationBuilder.DropTable(
                name: "TPNReportRequest");

            migrationBuilder.AddColumn<int>(
                name: "TPNCountryClientEmailId",
                table: "Country",
                type: "int",
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
        }
    }
}
