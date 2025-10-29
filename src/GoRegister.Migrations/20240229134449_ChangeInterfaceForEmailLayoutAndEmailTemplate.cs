using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ChangeInterfaceForEmailLayoutAndEmailTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLayout_Project_ProjectId",
                table: "EmailLayout");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplate_Project_ProjectId",
                table: "EmailTemplate");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLayout_Project_ProjectId",
                table: "EmailLayout",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplate_Project_ProjectId",
                table: "EmailTemplate",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLayout_Project_ProjectId",
                table: "EmailLayout");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplate_Project_ProjectId",
                table: "EmailTemplate");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLayout_Project_ProjectId",
                table: "EmailLayout",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplate_Project_ProjectId",
                table: "EmailTemplate",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
