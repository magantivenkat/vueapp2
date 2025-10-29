using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTableCustomPageVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomPageVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomPageId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomPageVersion_CustomPage_CustomPageId",
                        column: x => x.CustomPageId,
                        principalTable: "CustomPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bae1fa16-a61b-4587-8e86-cbb47d5f16eb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4a839f6-e8bb-42d4-9f96-f5a5a4ce2b75", "AQAAAAEAACcQAAAAEOFETd3+aMKb5aITmb089MIYcLdqY3+q6WhW1/9UeCjayIE+CB7D3uuI24iUgABHFw==" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageVersion_CustomPageId",
                table: "CustomPageVersion",
                column: "CustomPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomPageVersion");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "eb19ec05-dd4c-45fc-b4da-c0edde22ae19");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5cc102c4-4a88-44cd-80e9-3d4aaec4c742", "AQAAAAEAACcQAAAAEHsVPB5QnS6L2LAm5Hr3ru4M6GUeR7C9BOk3gLScA8kZuTXEiPxnDde6C5F6PA1OjA==" });
        }
    }
}
