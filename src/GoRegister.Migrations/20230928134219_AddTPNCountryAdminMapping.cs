using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTPNCountryAdminMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPNCountryAdminMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    ClientUuid = table.Column<string>(nullable: true),
                    TPNCountry = table.Column<string>(nullable: true),
                    AdminUserId = table.Column<int>(nullable: false),
                    GAMEmail = table.Column<string>(nullable: true),
                    ReportFrequency = table.Column<int>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPNCountryAdminMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPNCountryAdminMapping_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TPNCountryAdminMapping_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TPNCountryAdminMapping_ClientId",
                table: "TPNCountryAdminMapping",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TPNCountryAdminMapping_ModifiedByUserId",
                table: "TPNCountryAdminMapping",
                column: "ModifiedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPNCountryAdminMapping");
        }
    }
}
