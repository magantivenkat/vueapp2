using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SessionRegistrationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionRegistrationType",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRegistrationType", x => new { x.SessionId, x.RegistrationTypeId });
                    table.ForeignKey(
                        name: "FK_SessionRegistrationType_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRegistrationType_Session_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHvYxV8Mohc+K0Xficeytx6eNQw0EPKrUY9IjYKBFNVAhvRIT561QyAFAd2rzeGwyQ==");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRegistrationType_RegistrationTypeId",
                table: "SessionRegistrationType",
                column: "RegistrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionRegistrationType");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMp0lmoIaQgWCXJxwcRqvOyUqvFAZOwDRVqy7CZ+R23a4dbFIMG+MNJwWnr4QP/BVw==");
        }
    }
}
