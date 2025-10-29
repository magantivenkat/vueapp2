using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddjoinTableCustomPagesRegistrationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomPageRegistrationStatus",
                columns: table => new
                {
                    CustomPageId = table.Column<int>(nullable: false),
                    RegistrationStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageRegistrationStatus", x => new { x.CustomPageId, x.RegistrationStatusId });
                    table.ForeignKey(
                        name: "FK_CustomPageRegistrationStatus_CustomPage_CustomPageId",
                        column: x => x.CustomPageId,
                        principalTable: "CustomPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomPageRegistrationStatus_RegistrationStatus_RegistrationStatusId",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageRegistrationStatus_RegistrationStatusId",
                table: "CustomPageRegistrationStatus",
                column: "RegistrationStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomPageRegistrationStatus");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "37ec22f2-3f4b-48a9-980f-c3ef020cfe1b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3cd55b93-62c9-4e04-94a2-a6ec626c1d2b", "AQAAAAEAACcQAAAAEJ9pJMG9BpyH1nO30QVOia2TqfDX8OJ9gg2OZAVnzwY8BZRgT6WbxevdvOQyglX7tA==" });
        }
    }
}
