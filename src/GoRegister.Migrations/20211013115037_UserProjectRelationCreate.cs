using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class UserProjectRelationCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatedByUserId",
                table: "Project",
                column: "CreatedByUserId");

            migrationBuilder.SqlExec(
               @" UPDATE [dbo].[Project] set CreatedByUserId = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_CreatedByUserId",
                table: "Project",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_CreatedByUserId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_CreatedByUserId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Project");
        }
    }
}
