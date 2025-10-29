using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoRegister.Migrations
{
    public partial class MRFApprovalDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableApproval",
                table: "Field",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MRFApprovalActionDetails",
                columns: table => new
                {
                    MRFActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MRFClinetResponseId = table.Column<int>(type: "int", nullable: false),
                    UserGUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientGUID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ApproverEmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFApprovalActionDetails", x => x.MRFActionId);
                });

            migrationBuilder.CreateTable(
                name: "MRFApprovalRuleDetails",
                columns: table => new
                {
                    MRFApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalFieldId = table.Column<int>(type: "int", nullable: false),
                    ApproverEmailIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalRuleDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFApprovalRuleDetails", x => x.MRFApprovalId);
                    table.ForeignKey(
                        name: "FK_MRFApprovalRuleDetails_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MRFApprovalRuleDetails_ProjectId",
                table: "MRFApprovalRuleDetails",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MRFApprovalActionDetails");

            migrationBuilder.DropTable(
                name: "MRFApprovalRuleDetails");

            migrationBuilder.DropColumn(
                name: "EnableApproval",
                table: "Field");
        }
    }
}
