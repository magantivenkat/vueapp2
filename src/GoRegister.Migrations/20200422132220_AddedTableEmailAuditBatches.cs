using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddedTableEmailAuditBatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailAuditBatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    BatchId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    EmailSentCount = table.Column<int>(nullable: false),
                    EmailType = table.Column<string>(nullable: true),
                    DateSent = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAuditBatch", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAuditBatch");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "622101bc-ab81-4921-8e1f-dc8483782093");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b8f5a999-3626-42c2-9101-b0f8026aca99", "AQAAAAEAACcQAAAAEByg14IEEUdDj7HysPRF+qDp3JC1ESqUR1cjznwnRGTnabekV4vpFAgPsVb7PtVMqw==" });
        }
    }
}
