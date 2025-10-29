using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoRegister.Migrations
{
    public partial class MRFServiceCountryMappingDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MRFServiceCountryMapping",
                columns: table => new
                {
                    MappingCountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCountryUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestCountryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFServiceCountryMapping", x => x.MappingCountryId);
                    table.ForeignKey(
                        name: "FK_MRFServiceCountryMapping_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MRFServiceCountryMapping_ProjectId",
                table: "MRFServiceCountryMapping",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MRFServiceCountryMapping");
        }
    }
}
