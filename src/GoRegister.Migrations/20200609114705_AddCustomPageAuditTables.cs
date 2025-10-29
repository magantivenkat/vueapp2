using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddCustomPageAuditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "CustomPage",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CustomPageAudit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PageStatus = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomPageAuditRegistrationType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomPageAuditId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageAuditRegistrationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomPageAuditRegistrationType_CustomPageAudit_CustomPageAuditId",
                        column: x => x.CustomPageAuditId,
                        principalTable: "CustomPageAudit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c2cfa7e4-7e0a-41ea-9999-ca1358e5ebb5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e66572a8-1aa6-4665-a91c-eee307976b67", "AQAAAAEAACcQAAAAEND2zxE7GNC36jrEG7fInSv77w7wHElRMjUxLMqlH4VCxlsNO7xJDO2QCfwDeRjBFw==" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageAuditRegistrationType_CustomPageAuditId",
                table: "CustomPageAuditRegistrationType",
                column: "CustomPageAuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomPageAuditRegistrationType");

            migrationBuilder.DropTable(
                name: "CustomPageAudit");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "CustomPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "13afb0a0-277e-4ea3-b4ba-9b7df76a5b84");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1ce7f075-ef72-41f9-b68e-ec897f7e3ea5", "AQAAAAEAACcQAAAAELzgz2sAS3N47sEsmEoFKM1pWGwCRqaaj1rBbXVoU46rA4ujgSP3AdnhW87iX0UZ5Q==" });
        }
    }
}
