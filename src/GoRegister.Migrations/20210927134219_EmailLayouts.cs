using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class EmailLayouts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailLayoutId",
                table: "Email",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Html = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLayout_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Email_EmailLayoutId",
                table: "Email",
                column: "EmailLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLayout_ProjectId",
                table: "EmailLayout",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_EmailLayout_EmailLayoutId",
                table: "Email",
                column: "EmailLayoutId",
                principalTable: "EmailLayout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_EmailLayout_EmailLayoutId",
                table: "Email");

            migrationBuilder.DropTable(
                name: "EmailLayout");

            migrationBuilder.DropIndex(
                name: "IX_Email_EmailLayoutId",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "EmailLayoutId",
                table: "Email");
        }
    }
}
