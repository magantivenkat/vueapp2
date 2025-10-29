using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ProjectStatusAndCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Project",
                nullable: true);

            // generate 6 character code for existing projects
            migrationBuilder.Sql("UPDATE Project SET Code = (SELECT SUBSTRING(CONVERT(varchar(40), NEWID()), 0, 7))");


            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            // update statusId with isLive value
            migrationBuilder.Sql("UPDATE Project SET StatusId = IsLive");

            // drop islive - we dont need anymore
            migrationBuilder.DropColumn(
                name: "IsLive",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Project");
        }
    }
}
