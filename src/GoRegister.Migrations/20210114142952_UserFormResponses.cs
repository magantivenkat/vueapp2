using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class UserFormResponses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse");

            migrationBuilder.AddColumn<int>(
                name: "UserFormResponseId",
                table: "UserFieldResponse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserFormResponse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFormResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFormResponse_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFormResponse_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFormResponse_DelegateUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK5JJlOG8n12liXO8dL2U2Snp6XZotL8L7HjwO9/yc81GSxJHVIoWTLEUin+S5Vd3w==");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_UserFormResponseId",
                table: "UserFieldResponse",
                column: "UserFormResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFormResponse_FormId",
                table: "UserFormResponse",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFormResponse_ProjectId",
                table: "UserFormResponse",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFormResponse_UserId",
                table: "UserFormResponse",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse",
                column: "UserFormResponseId",
                principalTable: "UserFormResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse",
                column: "UserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_UserFormResponse_UserFormResponseId",
                table: "UserFieldResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse");

            migrationBuilder.DropTable(
                name: "UserFormResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserFieldResponse_UserFormResponseId",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "UserFormResponseId",
                table: "UserFieldResponse");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHwJeYBXLgKZW0bSyby4QLL0M6cFUTCWG8IOhlyEz5ZN0KaMlf4fycRYyul5vFCsNA==");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_DelegateUser_UserId",
                table: "UserFieldResponse",
                column: "UserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
