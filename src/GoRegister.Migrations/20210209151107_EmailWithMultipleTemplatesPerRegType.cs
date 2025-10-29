using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class EmailWithMultipleTemplatesPerRegType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmailType = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Cc = table.Column<string>(nullable: true),
                    Bcc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Email_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    BodyHtml = table.Column<string>(nullable: true),
                    HasTextBody = table.Column<bool>(nullable: false),
                    BodyText = table.Column<string>(nullable: true),
                    EmailId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplateRegistrationType",
                columns: table => new
                {
                    EmailTemplateId = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplateRegistrationType", x => new { x.EmailTemplateId, x.RegistrationTypeId });
                    table.ForeignKey(
                        name: "FK_EmailTemplateRegistrationType_EmailTemplate_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailTemplateRegistrationType_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplateRegistrationType_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEGEwrh4PQhl+5GG9iBKos9McXJtkmqnBhGH0r6Ulj5LQ1yqv5kb/0a/Upc6/42Noaw==");

            migrationBuilder.CreateIndex(
                name: "IX_Email_ProjectId",
                table: "Email",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_EmailId",
                table: "EmailTemplate",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_ProjectId",
                table: "EmailTemplate",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateRegistrationType_ProjectId",
                table: "EmailTemplateRegistrationType",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplateRegistrationType_RegistrationTypeId",
                table: "EmailTemplateRegistrationType",
                column: "RegistrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplateRegistrationType");

            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHDWuLFGWx7x1Aq6f79kz4+yzbWuQKaPS+PxRoWgVBxxN054eXY6g4XMDKckq0GgfQ==");
        }
    }
}
