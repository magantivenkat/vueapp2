using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AttendeeNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttendeeNumber",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.SqlExec("update [DelegateUser] set [AttendeeNumber] = NEWID()");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_AttendeeNumber_ProjectId",
                table: "DelegateUser",
                columns: new[] { "AttendeeNumber", "ProjectId" },
                unique: true,
                filter: "[AttendeeNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DelegateUser_AttendeeNumber_ProjectId",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "AttendeeNumber",
                table: "DelegateUser");
        }
    }
}
