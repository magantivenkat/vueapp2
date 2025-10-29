using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddEmailAuditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailAudit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DelegateUserId = table.Column<int>(nullable: false),
                    StatusId = table.Column<string>(nullable: true),
                    EmailTemplateId = table.Column<int>(nullable: true),
                    FromEmail = table.Column<string>(nullable: true),
                    FromEmailDisplayName = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Cc = table.Column<string>(nullable: true),
                    Bcc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                        column: x => x.DelegateUserId,
                        principalTable: "DelegateUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailAudit_EmailTemplate_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailAuditNotification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailAuditId = table.Column<int>(nullable: false),
                    SgMessageId = table.Column<string>(nullable: true),
                    SgEventId = table.Column<string>(nullable: true),
                    EventType = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    SmtpId = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    Attempt = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Tls = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AsmGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAuditNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAuditNotification_EmailAudit_EmailAuditId",
                        column: x => x.EmailAuditId,
                        principalTable: "EmailAudit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "24ee5723-bfe5-4a80-b1c5-44cd4a28c863");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d3d90f68-83ec-42e9-ba96-4e25b4226f94", "AQAAAAEAACcQAAAAEMMPJQWdPD01FHEaIJ0rCCaAB5mcHUyn6/Xip2cLEMhCZMd1iXwiVMfK60f53Y751Q==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAudit_DelegateUserId",
                table: "EmailAudit",
                column: "DelegateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAudit_EmailTemplateId",
                table: "EmailAudit",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuditNotification_EmailAuditId",
                table: "EmailAuditNotification",
                column: "EmailAuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAuditNotification");

            migrationBuilder.DropTable(
                name: "EmailAudit");

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
        }
    }
}
