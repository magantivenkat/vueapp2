using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveFieldCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_Field_FieldId",
                table: "UserFieldResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponseAudit_Field_FieldId",
                table: "UserFieldResponseAudit");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_Field_FieldId",
                table: "UserFieldResponse",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponseAudit_Field_FieldId",
                table: "UserFieldResponseAudit",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_Field_FieldId",
                table: "UserFieldResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponseAudit_Field_FieldId",
                table: "UserFieldResponseAudit");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_Field_FieldId",
                table: "UserFieldResponse",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponseAudit_Field_FieldId",
                table: "UserFieldResponseAudit",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
