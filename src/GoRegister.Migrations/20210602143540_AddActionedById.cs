using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddActionedById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUserAudit_User_ActionedById",
                table: "DelegateUserAudit");

            migrationBuilder.AlterColumn<int>(
                name: "ActionedById",
                table: "DelegateUserAudit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMcdei2X5jIM3FEhQNvsBL6m2KLfYtipWFSHJBsWCqInbIgGbuIQ/J695VeWSVTwAQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUserAudit_User_ActionedById",
                table: "DelegateUserAudit",
                column: "ActionedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUserAudit_User_ActionedById",
                table: "DelegateUserAudit");

            migrationBuilder.AlterColumn<int>(
                name: "ActionedById",
                table: "DelegateUserAudit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDNCGcc8SSjjhQC/nK5hkbySIdz0Nih+L1LhEsu5nA4dSa/eGjhcpOgZJLQOtFYRGg==");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUserAudit_User_ActionedById",
                table: "DelegateUserAudit",
                column: "ActionedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
