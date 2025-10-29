using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoRegister.Migrations
{
    public partial class MRFLanguage_Feature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MRFLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MRFLanguage_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MRFLanguage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MRFLanguageResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRFLanguageId = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    LanguageResource = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFLanguageResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MRFLanguageResource_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MRFLanguageResource_MRFLanguage_MRFLanguageId",
                        column: x => x.MRFLanguageId,
                        principalTable: "MRFLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MRFLanguage_LanguageId",
                table: "MRFLanguage",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MRFLanguage_ProjectId",
                table: "MRFLanguage",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MRFLanguageResource_FieldId",
                table: "MRFLanguageResource",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_MRFLanguageResource_MRFLanguageId",
                table: "MRFLanguageResource",
                column: "MRFLanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MRFLanguageResource");

            migrationBuilder.DropTable(
                name: "MRFLanguage");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
