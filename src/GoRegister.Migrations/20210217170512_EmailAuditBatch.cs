using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class EmailAuditBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "EmailSentCount",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "EmailType",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailAuditBatch");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "EmailAuditBatch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreatedUtc",
                table: "EmailAuditBatch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSentUtc",
                table: "EmailAuditBatch",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailCount",
                table: "EmailAuditBatch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmailId",
                table: "EmailAuditBatch",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailIssueCount",
                table: "EmailAuditBatch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EmailAuditBatch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "EmailAudit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DelegateUserId",
                table: "EmailAudit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreatedUtc",
                table: "EmailAudit",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmailAuditBatchId",
                table: "EmailAudit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateId",
                table: "EmailAudit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEO/MwQuYXRyIXRwX6v4K/NxTmPBDd+VkYwLBUKhFnvr3mt4sN5v7bZRX04bG9v//Pg==");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuditBatch_CreatedByUserId",
                table: "EmailAuditBatch",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAuditBatch_EmailId",
                table: "EmailAuditBatch",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAudit_EmailAuditBatchId",
                table: "EmailAudit",
                column: "EmailAuditBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAudit_EmailTemplateId",
                table: "EmailAudit",
                column: "EmailTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit",
                column: "DelegateUserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_EmailAuditBatch_EmailAuditBatchId",
                table: "EmailAudit",
                column: "EmailAuditBatchId",
                principalTable: "EmailAuditBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_EmailTemplate_EmailTemplateId",
                table: "EmailAudit",
                column: "EmailTemplateId",
                principalTable: "EmailTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAuditBatch_User_CreatedByUserId",
                table: "EmailAuditBatch",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAuditBatch_Email_EmailId",
                table: "EmailAuditBatch",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_EmailAuditBatch_EmailAuditBatchId",
                table: "EmailAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailAudit_EmailTemplate_EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailAuditBatch_User_CreatedByUserId",
                table: "EmailAuditBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailAuditBatch_Email_EmailId",
                table: "EmailAuditBatch");

            migrationBuilder.DropIndex(
                name: "IX_EmailAuditBatch_CreatedByUserId",
                table: "EmailAuditBatch");

            migrationBuilder.DropIndex(
                name: "IX_EmailAuditBatch_EmailId",
                table: "EmailAuditBatch");

            migrationBuilder.DropIndex(
                name: "IX_EmailAudit_EmailAuditBatchId",
                table: "EmailAudit");

            migrationBuilder.DropIndex(
                name: "IX_EmailAudit_EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "DateCreatedUtc",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "DateSentUtc",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "EmailCount",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "EmailIssueCount",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmailAuditBatch");

            migrationBuilder.DropColumn(
                name: "DateCreatedUtc",
                table: "EmailAudit");

            migrationBuilder.DropColumn(
                name: "EmailAuditBatchId",
                table: "EmailAudit");

            migrationBuilder.DropColumn(
                name: "EmailTemplateId",
                table: "EmailAudit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "EmailAuditBatch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmailSentCount",
                table: "EmailAuditBatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmailType",
                table: "EmailAuditBatch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EmailAuditBatch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "EmailAudit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DelegateUserId",
                table: "EmailAudit",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDqRSOPy7lJFhCUo0OGSTWSq8kPFMs86BT4lSRleOYupqt6gjjxxBl9yT9d1/XW4xg==");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAudit_DelegateUser_DelegateUserId",
                table: "EmailAudit",
                column: "DelegateUserId",
                principalTable: "DelegateUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
