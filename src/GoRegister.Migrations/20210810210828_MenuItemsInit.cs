using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MenuItemsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MenuItemType = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    OpenInNewTab = table.Column<bool>(nullable: false),
                    CssClass = table.Column<string>(nullable: true),
                    AnchorLink = table.Column<string>(nullable: true),
                    CustomPageId = table.Column<int>(nullable: true),
                    FormType = table.Column<int>(nullable: true),
                    FormId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_CustomPage_CustomPageId",
                        column: x => x.CustomPageId,
                        principalTable: "CustomPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItem_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItem_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemRegistrationType",
                columns: table => new
                {
                    RegistrationTypeId = table.Column<int>(nullable: false),
                    MenuItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemRegistrationType", x => new { x.MenuItemId, x.RegistrationTypeId });
                    table.ForeignKey(
                        name: "FK_MenuItemRegistrationType_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemRegistrationType_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_CustomPageId",
                table: "MenuItem",
                column: "CustomPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_FormId",
                table: "MenuItem",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_ProjectId",
                table: "MenuItem",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemRegistrationType_RegistrationTypeId",
                table: "MenuItemRegistrationType",
                column: "RegistrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemRegistrationType");

            migrationBuilder.DropTable(
                name: "MenuItem");
        }
    }
}
