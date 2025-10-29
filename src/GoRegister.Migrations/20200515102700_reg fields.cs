using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class regfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Field_FieldType_FieldTypeId", "Field");
            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(name: "FK_Field_FieldType_FieldTypeId",
                table: "Field",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


            migrationBuilder.RenameColumn(
                name: "Value",
                table: "UserFieldResponse",
                newName: "StringValue");

            migrationBuilder.AddColumn<bool>(
                name: "BooleanValue",
                table: "UserFieldResponse",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeValue",
                table: "UserFieldResponse",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NumberValue",
                table: "UserFieldResponse",
                nullable: false,
                defaultValue: 0);




            migrationBuilder.AddColumn<string>(
                name: "RegistrationDocument",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.InsertData(
                table: "FieldType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Textbox" },
                    { 10, "PhoneNumber" },
                    { 2, "RadioButton" },
                    { 4, "TextArea" },
                    { 5, "Date" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "BooleanValue",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "DateTimeValue",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "NumberValue",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "RegistrationDocument",
                table: "DelegateUser");

            migrationBuilder.RenameColumn(
                name: "StringValue",
                table: "UserFieldResponse",
                newName: "Value");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "FieldType",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsForPresentation",
                table: "FieldType",
                nullable: false,
                defaultValue: false);}
    }
}
