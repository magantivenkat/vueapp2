using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class delegateuserdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDsxFVsEyj5wpK4XE/TRj4cgHm5ykkR02oMnhIakdFpW7JXDi3DMUSmW0Fy3tAOZ/w==");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit",
                column: "DelegateUserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELr5Dc0AmJ7jYvSN+4n+wq9GRgrxThnIFGOfuAp3cpzh3FqZ6s+WRXbibtWrR4vqog==");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit",
                column: "DelegateUserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
