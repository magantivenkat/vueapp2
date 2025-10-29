using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MakeFieldResponsesNullableAndRemoveDelegateLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserFieldResponse_UserId",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFieldResponse");

            migrationBuilder.AlterColumn<int>(
                name: "NumberValue",
                table: "UserFieldResponse",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeValue",
                table: "UserFieldResponse",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "BooleanValue",
                table: "UserFieldResponse",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDXVwVWZ19Dq8HZLewpgCTF+b96PO2hv4+KwKI2VhqiD+8M47JAF2Lsh2yz7uFFSKA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberValue",
                table: "UserFieldResponse",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeValue",
                table: "UserFieldResponse",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "BooleanValue",
                table: "UserFieldResponse",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserFieldResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAENMxgHDbWahiIVUJ45rbkDmV9buPnAWwi3zzr152bFJMjTXVbtSh6sTQA6jP6FfPgQ==");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_UserId",
                table: "UserFieldResponse",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse",
                column: "UserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
