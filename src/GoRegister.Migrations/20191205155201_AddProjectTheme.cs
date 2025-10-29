using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddProjectTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTheme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LayoutName = table.Column<string>(nullable: true),
                    ThemeVariables = table.Column<string>(nullable: true),
                    ThemeVariableObject = table.Column<string>(nullable: true),
                    ThemeCss = table.Column<string>(nullable: true),
                    OverrideCss = table.Column<string>(nullable: true),
                    HeadScripts = table.Column<string>(nullable: true),
                    FooterScripts = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTheme_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d1a20240-06af-4df6-aef7-fd8780561e12");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "afc335c3-6c4c-494f-9f93-86c2514aae69", "AQAAAAEAACcQAAAAEMzU83vWYJJmvUKZKoN/4O9mzQySqG7bKVgqWMmBllg+TVPlsXSPTc641lnjnc+eOA==" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTheme_ProjectId",
                table: "ProjectTheme",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTheme");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "633f2b07-95cd-49b1-b2d4-19dd20506c4d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f6087421-81ae-4ee5-8971-50f6882caf53", "AQAAAAEAACcQAAAAEKbETs82F9GT8mPRPfQMFUeO4BiTz+lwFDh0UbjC6fZFVK+LPGfnASQlkqXsCVa4Iw==" });
        }
    }
}
