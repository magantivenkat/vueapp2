using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveEmailTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_EmailTemplate_EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.DropIndex(
                name: "IX_EmailAudit_EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.DropColumn(
                name: "EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHDWuLFGWx7x1Aq6f79kz4+yzbWuQKaPS+PxRoWgVBxxN054eXY6g4XMDKckq0GgfQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateId",
                table: "EmailAudit",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bcc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailType = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    RegistrationStatusId = table.Column<int>(type: "int", nullable: false),
                    RegistrationTypeId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_RegistrationStatus_RegistrationStatusId",
                        column: x => x.RegistrationStatusId,
                        principalTable: "RegistrationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_RegistrationType_RegistrationTypeId",
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
                value: "AQAAAAEAACcQAAAAEIfe3RAdKdwvcFfwyzof5EBXQm1kfEWoIE6R/h7klaMe3FK5ulD4O6u6HoGtrwRP2w==");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAudit_EmailTemplateId",
                table: "EmailAudit",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_ProjectId",
                table: "EmailTemplate",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_RegistrationStatusId",
                table: "EmailTemplate",
                column: "RegistrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_RegistrationTypeId",
                table: "EmailTemplate",
                column: "RegistrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_EmailTemplate_EmailTemplateId",
                table: "EmailAudit",
                column: "EmailTemplateId",
                principalTable: "EmailTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
