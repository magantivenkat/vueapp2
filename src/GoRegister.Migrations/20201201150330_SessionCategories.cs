using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class SessionCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionCategoryId",
                table: "Session",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SessionCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsSingleSession = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionCategory_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMp0lmoIaQgWCXJxwcRqvOyUqvFAZOwDRVqy7CZ+R23a4dbFIMG+MNJwWnr4QP/BVw==");

            migrationBuilder.CreateIndex(
                name: "IX_Session_SessionCategoryId",
                table: "Session",
                column: "SessionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionCategory_ProjectId",
                table: "SessionCategory",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_SessionCategory_SessionCategoryId",
                table: "Session",
                column: "SessionCategoryId",
                principalTable: "SessionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_SessionCategory_SessionCategoryId",
                table: "Session");

            migrationBuilder.DropTable(
                name: "SessionCategory");

            migrationBuilder.DropIndex(
                name: "IX_Session_SessionCategoryId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "SessionCategoryId",
                table: "Session");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDiB04y8Stf6Kiq1vTkavOczCJyGtDHCoRIMjotI1yfHatZ/DbPtpuZWafdyMd95UA==");
        }
    }
}
