using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class CleanUpField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_RegistrationPage_RegistrationPageId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "ApiFieldName",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "CustomAttributes",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "MaxLength",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "MinLength",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "PreText",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "SubText",
                table: "Field");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationPageId",
                table: "Field",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ae6d23bd-3e34-43eb-aabc-f3963b993149");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4a0f598a-0229-4604-8e6d-35318fd4b9a6", "AQAAAAEAACcQAAAAEOG5R0nWXof19wg9dSfDxPAFPtkTAJvtcH+AoDIwuDha596DQcJfOsM4dlIykM/rcA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Field_RegistrationPage_RegistrationPageId",
                table: "Field",
                column: "RegistrationPageId",
                principalTable: "RegistrationPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_RegistrationPage_RegistrationPageId",
                table: "Field");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationPageId",
                table: "Field",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiFieldName",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomAttributes",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxLength",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinLength",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreText",
                table: "Field",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubText",
                table: "Field",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "30070246-c935-4077-b278-bb6ccb917066");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e575f5f6-7b7a-4a21-9d4e-99324b45dd78", "AQAAAAEAACcQAAAAEHlVhxGjRz2ufcucke020spRwwKW59xW/qrIER0qtuzmgJ/7qL5PsDDAB11/4edCxQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Field_RegistrationPage_RegistrationPageId",
                table: "Field",
                column: "RegistrationPageId",
                principalTable: "RegistrationPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
