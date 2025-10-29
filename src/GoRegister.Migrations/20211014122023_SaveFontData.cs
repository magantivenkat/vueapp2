using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SaveFontData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThemeFont",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    FontType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Variants = table.Column<string>(nullable: true),
                    ProjectThemeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeFont", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemeFont_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThemeFont_ProjectTheme_ProjectThemeId",
                        column: x => x.ProjectThemeId,
                        principalTable: "ProjectTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThemeFont_ProjectId",
                table: "ThemeFont",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeFont_ProjectThemeId",
                table: "ThemeFont",
                column: "ProjectThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeFont");
        }
    }
}
