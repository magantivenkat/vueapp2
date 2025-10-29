using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class FormAuditing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserFormResponseId",
                table: "DelegateUserAudit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIfe3RAdKdwvcFfwyzof5EBXQm1kfEWoIE6R/h7klaMe3FK5ulD4O6u6HoGtrwRP2w==");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUserAudit_UserFormResponseId",
                table: "DelegateUserAudit",
                column: "UserFormResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUserAudit_UserFormResponse_UserFormResponseId",
                table: "DelegateUserAudit",
                column: "UserFormResponseId",
                principalTable: "UserFormResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUserAudit_UserFormResponse_UserFormResponseId",
                table: "DelegateUserAudit");

            migrationBuilder.DropIndex(
                name: "IX_DelegateUserAudit_UserFormResponseId",
                table: "DelegateUserAudit");

            migrationBuilder.DropColumn(
                name: "UserFormResponseId",
                table: "DelegateUserAudit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIMhyi8blPVLieYUVS22FRSc7xS4HnjCjDgAKtw2i0VG7ULqHII1GRacLuK4Jj9O3w==");
        }
    }
}
