using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RecordBulkUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BulkUpload",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateUploadedUtc = table.Column<DateTime>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulkUpload_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BulkUpload_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulkUpload_ProjectId",
                table: "BulkUpload",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkUpload_UserId",
                table: "BulkUpload",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulkUpload");
        }
    }
}
