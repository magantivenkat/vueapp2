using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateSessionBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DelegateSessionBooking",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false),
                    DelegateUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateSessionBooking", x => new { x.SessionId, x.DelegateUserId });
                    table.ForeignKey(
                        name: "FK_DelegateSessionBooking_DelegateUser_DelegateUserId",
                        column: x => x.DelegateUserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegateSessionBooking_Session_SessionId",
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
                value: "AQAAAAEAACcQAAAAEK6gRjSaPvFL2BvK7AsmM02V7v//FoYlBgqV1R4eznO91EesFkAZ8PI6azoG7dNkSw==");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateSessionBooking_DelegateUserId",
                table: "DelegateSessionBooking",
                column: "DelegateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DelegateSessionBooking");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHvYxV8Mohc+K0Xficeytx6eNQw0EPKrUY9IjYKBFNVAhvRIT561QyAFAd2rzeGwyQ==");
        }
    }
}
