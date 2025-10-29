using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTableEmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplate",
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
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_InvitationList_InvitationListId",
                        column: x => x.InvitationListId,
                        principalTable: "InvitationList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bcfbf78c-7434-4f1a-9dde-278b365a9152");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b71a9286-79a5-4fd8-8011-255282d0af3c", "AQAAAAEAACcQAAAAEMxS5Lp1TLtJgxnFNx4exj23Ew1/AhgIGvHJFw/xz7ZL7La3Tw0t6wwSM6hhTNiddQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_InvitationListId",
                table: "EmailTemplate",
                column: "InvitationListId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "02102bf6-62a3-4f85-95fb-91712e8e100f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1d643ad8-fd8f-41b0-b64a-aa642f4b95fd", "AQAAAAEAACcQAAAAEPxat67Bm5bXZJQXU9SimVkuZttu9/ENgj+gXrR91asiaSxfJuD11hvIQu7nUrWSpA==" });
        }
    }
}
