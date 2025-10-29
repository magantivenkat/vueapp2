using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableRegistrationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.AlterColumn<int>(
                name: "InvitationListId",
                table: "RegistrationType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d59e5010-7b6d-4ff1-b70b-8f21e26c1650");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "777c8614-b042-4784-a182-dd1e307b2af3", "AQAAAAEAACcQAAAAENv8tfvqL4eNd9wX/KiD7r6GqKqZivOfqIvj1tTTaqRQnuPQBBkDgODEB+IBpujqdA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.AlterColumn<int>(
                name: "InvitationListId",
                table: "RegistrationType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1aaf03dc-5a96-4cbc-ba21-29aa6b72e929");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "97603534-28d2-4d0e-95e8-ccae9f628d81", "AQAAAAEAACcQAAAAEKrReS6yGK5BNa13JpomqtxYfdWuZRuAEN0k3UHKyLbpDXUDVaV6TymTZNWi1XYClg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
