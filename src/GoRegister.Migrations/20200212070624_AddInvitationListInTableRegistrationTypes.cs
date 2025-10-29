using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddInvitationListInTableRegistrationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationListId",
                table: "RegistrationType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.SqlExec(
                @"  UPDATE dbo.RegistrationType
                    SET
	                    invitationlistid = invitationlist.Id
                    FROM 
	                    RegistrationType
	                    Cross APPLY (
                            SELECT TOP 1 il.* FROM dbo.InvitationList il WHERE il.ProjectId = dbo.RegistrationType.ProjectId
                        ) AS invitationlist");

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

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationType_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationType_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.DropColumn(
                name: "InvitationListId",
                table: "RegistrationType");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d5530219-9060-4909-a630-9350211cad20");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e1230667-e26b-4ab2-8e48-e05db98ec187", "AQAAAAEAACcQAAAAELTM0RASLeQ9sxy9znc1TMWwtyYsGhWExy1HrzEtDXac3qkAm7r2qZPDqY7yRxrg6g==" });
        }
    }
}
