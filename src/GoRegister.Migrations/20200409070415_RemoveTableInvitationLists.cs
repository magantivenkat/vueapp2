using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveTableInvitationLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUser_InvitationList_InvitationListId",
                table: "DelegateUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailTemplate_InvitationList_InvitationListId",
                table: "EmailTemplate");

            //TODO: can we remove all migrations related to EmailManagementTemplate?
            //migrationBuilder.DropTable(
                //name: "EmailManagementTemplate");

            migrationBuilder.DropTable(
                name: "InvitationList");

            migrationBuilder.DropIndex(
                name: "IX_EmailTemplate_InvitationListId",
                table: "EmailTemplate");

            migrationBuilder.DropIndex(
                name: "IX_DelegateUser_InvitationListId",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "InvitationListId",
                table: "EmailTemplate");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmailTemplate");

            migrationBuilder.DropColumn(
                name: "InvitationListId",
                table: "DelegateUser");

            migrationBuilder.AddColumn<int>(
                name: "EmailType",
                table: "EmailTemplate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0639693f-8ab2-4387-94fa-38336e9c3bf9");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6d22f8c2-0757-4542-9ccf-b8d60ca4abf2", "AQAAAAEAACcQAAAAEHOkbGTRxJrzrQJGvUg/PZ39dJ2gKDuI28cuxMyJN9bK3MoFaesXParYtVE8Fw5/Cw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailType",
                table: "EmailTemplate");

            migrationBuilder.AddColumn<int>(
                name: "InvitationListId",
                table: "EmailTemplate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EmailTemplate",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvitationListId",
                table: "DelegateUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InvitationList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitationList_Project_ProjectId",
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
                value: "e6f997b7-a43e-442b-8cb9-e471bb1d6b10");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7c92f9b1-0e9a-4e8c-8df3-c5f702a3970c", "AQAAAAEAACcQAAAAEKkGL3BVA/nIUWWRgWT/H+CipGmtT9yrEB5vedTe27JjCxjv1ekjtQD8LRTi7Ra8NA==" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_InvitationListId",
                table: "EmailTemplate",
                column: "InvitationListId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_InvitationListId",
                table: "DelegateUser",
                column: "InvitationListId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationList_ProjectId",
                table: "InvitationList",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUser_InvitationList_InvitationListId",
                table: "DelegateUser",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailTemplate_InvitationList_InvitationListId",
                table: "EmailTemplate",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
