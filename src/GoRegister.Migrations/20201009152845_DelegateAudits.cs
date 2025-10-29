using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateAudits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DelegateUserAudit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ActionById = table.Column<int>(nullable: true),
                    ActionedFrom = table.Column<int>(nullable: false),
                    ActionedUtc = table.Column<DateTime>(nullable: false),
                    RegistrationStatusId = table.Column<int>(nullable: true),
                    RegistrationTypeId = table.Column<int>(nullable: true),
                    RegistrationTypeName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    ActionedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegateUserAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DelegateUserAudit_User_ActionedById",
                        column: x => x.ActionedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegateUserAudit_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegateUserAudit_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DelegateUserAudit_DelegateUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFieldResponseAudit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    FieldOptionId = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(nullable: true),
                    NumberValue = table.Column<int>(nullable: true),
                    BooleanValue = table.Column<bool>(nullable: true),
                    DateTimeValue = table.Column<DateTime>(nullable: true),
                    DelegateUserAuditId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFieldResponseAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFieldResponseAudit_DelegateUserAudit_DelegateUserAuditId",
                        column: x => x.DelegateUserAuditId,
                        principalTable: "DelegateUserAudit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponseAudit_Field_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFieldResponseAudit_FieldOption_FieldOptionId",
                        column: x => x.FieldOptionId,
                        principalTable: "FieldOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponseAudit_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFieldResponseAudit_DelegateUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b95d7183-c3b9-465f-98c8-3523bffee73a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a7511e79-4868-43cd-b483-1e6c437b56c4", "AQAAAAEAACcQAAAAECfiSPTPkSfxwZtFbsmx7IYlWW1s0090JtuqHnz8F2GH8ra7RrCU/DPMBaD3iXPpCQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUserAudit_ActionedById",
                table: "DelegateUserAudit",
                column: "ActionedById");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUserAudit_ProjectId",
                table: "DelegateUserAudit",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUserAudit_RegistrationTypeId",
                table: "DelegateUserAudit",
                column: "RegistrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUserAudit_UserId",
                table: "DelegateUserAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponseAudit_DelegateUserAuditId",
                table: "UserFieldResponseAudit",
                column: "DelegateUserAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponseAudit_FieldId",
                table: "UserFieldResponseAudit",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponseAudit_FieldOptionId",
                table: "UserFieldResponseAudit",
                column: "FieldOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponseAudit_ProjectId",
                table: "UserFieldResponseAudit",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponseAudit_UserId",
                table: "UserFieldResponseAudit",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFieldResponseAudit");

            migrationBuilder.DropTable(
                name: "DelegateUserAudit");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "71880ff1-fdb1-4240-87c8-bc22b59b8317");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3d341ca9-a111-4641-a680-235c789e55d7", "AQAAAAEAACcQAAAAEHG1t4dvJNq2U482/tsk/58Z9DnqCCfXf/iDcGJ++Af0MgUgMEq+5+jBEUtZsqtecw==" });
        }
    }
}
