using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class deletedelegate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELr5Dc0AmJ7jYvSN+4n+wq9GRgrxThnIFGOfuAp3cpzh3FqZ6s+WRXbibtWrR4vqog==");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse",
                column: "UserFormResponseId",
                principalTable: "UserFormResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAENTqaCsPfY+yzMM9JpOlju4NPEc4xxsKjOAx03ALXyTmKOJGnBYTovXbQdaKH45eXQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse",
                column: "UserFormResponseId",
                principalTable: "UserFormResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
