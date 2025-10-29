using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTableEmailManagementTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            return;

            migrationBuilder.CreateTable(
                name: "EmailManagementTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    RegistrationTypeId = table.Column<int>(nullable: false),
                    InvitationListId = table.Column<int>(nullable: false),
                    RegistrationStatusId = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Cc = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailManagementTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailManagementTemplate_Project_ProjectId",
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
                value: "22bdaf08-2422-4ab9-ac2b-eb0289474c6e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b67293d5-04b0-49fa-9ee0-acd1d85d1b6e", "AQAAAAEAACcQAAAAEDj5Os+8Bn7mF+AXBOmY/qbBXBMNoGLbu60rYHFmC1cpwF/LRR7xuTkdgbgxs831PQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailManagementTemplate_ProjectId",
                table: "EmailManagementTemplate",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailManagementTemplate");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d3019239-e7cf-4709-aeea-3a9e15b83ea3");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3c9f7edb-de95-4d4a-83e2-74a12aa2a35e", "AQAAAAEAACcQAAAAENBONY3CLzPcGhrxVXm1bNUnkN4/flmc5JKH+VHG0ccdscW7m8SkqbKLdotDwxY50A==" });
        }
    }
}
