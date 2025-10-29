using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateUserMustHaveProjectEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "DelegateUser",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPsydlIlOx8M+45SY2wQ2ukAb3egYYQ3VF+JwbPMvvpkg2h2NSKjTR7hvWu48Ir5Fw==");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_ProjectId",
                table: "DelegateUser",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUser_Project_ProjectId",
                table: "DelegateUser",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUser_Project_ProjectId",
                table: "DelegateUser");

            migrationBuilder.DropIndex(
                name: "IX_DelegateUser_ProjectId",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPt/Z7PecWQNHEh4lxn4v8dQwCdnB1sKgVMMfwL9y3VrNp7WRSiYbAPtryaqAh9Sfw==");
        }
    }
}
