using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AnonSessionBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnonSessionBooking",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false),
                    AnonUserId = table.Column<string>(nullable: false),
                    ActionedByUserId = table.Column<int>(nullable: false),
                    DateActionedUtc = table.Column<DateTime>(nullable: false),
                    DateReleasedUtc = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnonSessionBooking", x => new { x.SessionId, x.AnonUserId });
                    table.ForeignKey(
                        name: "FK_AnonSessionBooking_Session_SessionId",
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
                value: "AQAAAAEAACcQAAAAEPt/Z7PecWQNHEh4lxn4v8dQwCdnB1sKgVMMfwL9y3VrNp7WRSiYbAPtryaqAh9Sfw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnonSessionBooking");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDqRSOPy7lJFhCUo0OGSTWSq8kPFMs86BT4lSRleOYupqt6gjjxxBl9yT9d1/XW4xg==");
        }
    }
}
