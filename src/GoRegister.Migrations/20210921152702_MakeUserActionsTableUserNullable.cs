using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MakeUserActionsTableUserNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAction",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAction",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAction_User_UserId",
                table: "UserAction",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
