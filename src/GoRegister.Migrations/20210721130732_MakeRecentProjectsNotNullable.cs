using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MakeRecentProjectsNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentProject_Project_ProjectId",
                table: "RecentProject");

            migrationBuilder.DropForeignKey(
                name: "FK_RecentProject_User_UserId",
                table: "RecentProject");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecentProject",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "RecentProject",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentProject_Project_ProjectId",
                table: "RecentProject",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentProject_User_UserId",
                table: "RecentProject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentProject_Project_ProjectId",
                table: "RecentProject");

            migrationBuilder.DropForeignKey(
                name: "FK_RecentProject_User_UserId",
                table: "RecentProject");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecentProject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "RecentProject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RecentProject_Project_ProjectId",
                table: "RecentProject",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecentProject_User_UserId",
                table: "RecentProject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
