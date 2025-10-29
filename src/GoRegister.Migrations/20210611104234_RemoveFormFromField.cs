using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveFormFromField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field");

            migrationBuilder.AlterColumn<int>(
                name: "FormId",
                table: "Field",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMoky7mpLD5YYehLC6iQ2CTGrsGncQRBhicYnUThEWQ9+S0Ppzi83O1H2AeMBhAPmA==");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field");

            migrationBuilder.AlterColumn<int>(
                name: "FormId",
                table: "Field",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFgk8PbTLtuqmVoNmbCjWCMjWfdGqUiFvQU3WKAUp6uo2LSU+0tCZpXaTPRdXBHJtA==");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
