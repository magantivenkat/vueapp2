using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveFieldCollectionFromForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Field_FormId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEBqzEbpcdeDB/HdCH23ZlxwgfrYnaeEqASFghmbo7o3hQvo75lbbWpjrcNeQ/uN7Lg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "Field",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEJvaGib8+LlH7SONDqmARSAO+xKobgMD1uq8dztNFrACnKzIcv8qr1IrRM2vY5vt/Q==");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormId",
                table: "Field",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
