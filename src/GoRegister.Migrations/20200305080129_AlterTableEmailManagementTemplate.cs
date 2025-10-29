using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableEmailManagementTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            return;

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "74b8c4eb-6c62-4470-9a72-f4faaa806fb3");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4fb21c6d-b166-4402-8202-eb522a261185", "AQAAAAEAACcQAAAAEM8MTqyA7wOzZQXybI+h4wRfWLvAlVfNveUtX8kSZpj88+lNiqXJBNNRUJVnQY6BKA==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailManagementTemplate_InvitationListId",
                table: "EmailManagementTemplate",
                column: "InvitationListId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailManagementTemplate_RegistrationStatusId",
                table: "EmailManagementTemplate",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailManagementTemplate_RegistrationTypeId",
                table: "EmailManagementTemplate",
                column: "RegistrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailManagementTemplate_InvitationList_InvitationListId",
                table: "EmailManagementTemplate",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailManagementTemplate_RegistrationStatus_RegistrationStatusId",
                table: "EmailManagementTemplate",
                column: "RegistrationStatusId",
                principalTable: "RegistrationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailManagementTemplate_RegistrationType_RegistrationTypeId",
                table: "EmailManagementTemplate",
                column: "RegistrationTypeId",
                principalTable: "RegistrationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailManagementTemplate_InvitationList_InvitationListId",
                table: "EmailManagementTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailManagementTemplate_RegistrationStatus_RegistrationStatusId",
                table: "EmailManagementTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailManagementTemplate_RegistrationType_RegistrationTypeId",
                table: "EmailManagementTemplate");

            migrationBuilder.DropIndex(
                name: "IX_EmailManagementTemplate_InvitationListId",
                table: "EmailManagementTemplate");

            migrationBuilder.DropIndex(
                name: "IX_EmailManagementTemplate_RegistrationStatusId",
                table: "EmailManagementTemplate");

            migrationBuilder.DropIndex(
                name: "IX_EmailManagementTemplate_RegistrationTypeId",
                table: "EmailManagementTemplate");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "22bdaf08-2422-4ab9-ac2b-eb0289474c6e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b67293d5-04b0-49fa-9ee0-acd1d85d1b6e", "AQAAAAEAACcQAAAAEDj5Os+8Bn7mF+AXBOmY/qbBXBMNoGLbu60rYHFmC1cpwF/LRR7xuTkdgbgxs831PQ==" });
        }
    }
}
