using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddProjectIdToTableEmailAuditBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "53583c28-8556-4b5e-a395-525bae80000f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c44be46e-f322-4374-adf6-742ca6885a3a", "AQAAAAEAACcQAAAAEGGoDwT/qZ+0PJ2YFClQlFDqTEHEUlqXle+OK9w5XBtPa5aL56ZuC0XOpkf8a3BXQw==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuditBatch_ProjectId",
                table: "EmailAuditBatch",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAuditBatch_Project_ProjectId",
                table: "EmailAuditBatch",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAuditBatch_Project_ProjectId",
                table: "EmailAuditBatch");

            migrationBuilder.DropIndex(
                name: "IX_EmailAuditBatch_ProjectId",
                table: "EmailAuditBatch");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "aaf310ce-9710-4d95-8fd2-bbb1b1092b82");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9898030d-4822-49ca-b9cc-0deec9d7b7b4", "AQAAAAEAACcQAAAAEDJ1pzBnoikbTK5rux8JDYFSe9PFyHfHCgax6uBXpX08++hlOzjPeal8HJZflP5q/Q==" });
        }
    }
}
