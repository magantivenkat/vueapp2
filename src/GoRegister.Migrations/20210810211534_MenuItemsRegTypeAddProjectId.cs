using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MenuItemsRegTypeAddProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "MenuItemRegistrationType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemRegistrationType_ProjectId",
                table: "MenuItemRegistrationType",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemRegistrationType_Project_ProjectId",
                table: "MenuItemRegistrationType",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemRegistrationType_Project_ProjectId",
                table: "MenuItemRegistrationType");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemRegistrationType_ProjectId",
                table: "MenuItemRegistrationType");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "MenuItemRegistrationType");
        }
    }
}
