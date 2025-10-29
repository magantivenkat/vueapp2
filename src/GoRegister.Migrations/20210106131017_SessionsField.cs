using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SessionsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideFullSessions",
                table: "Field",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SessionFieldCategory",
                columns: table => new
                {
                    SessionFieldId = table.Column<int>(nullable: false),
                    SessionCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionFieldCategory", x => new { x.SessionFieldId, x.SessionCategoryId });
                    table.ForeignKey(
                        name: "FK_SessionFieldCategory_SessionCategory_SessionCategoryId",
                        column: x => x.SessionCategoryId,
                        principalTable: "SessionCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionFieldCategory_Field_SessionFieldId",
                        column: x => x.SessionFieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEEAWijDnzrstm9S2+mKeTIRtQwokbywIOPYMSXTo0qYgbcbNgqocoE2zY9W/vckY0A==");

            migrationBuilder.CreateIndex(
                name: "IX_SessionFieldCategory_SessionCategoryId",
                table: "SessionFieldCategory",
                column: "SessionCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionFieldCategory");

            migrationBuilder.DropColumn(
                name: "HideFullSessions",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK6gRjSaPvFL2BvK7AsmM02V7v//FoYlBgqV1R4eznO91EesFkAZ8PI6azoG7dNkSw==");
        }
    }
}
