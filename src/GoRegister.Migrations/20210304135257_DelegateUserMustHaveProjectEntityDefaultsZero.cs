using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateUserMustHaveProjectEntityDefaultsZero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
     name: "ProjectId",
     table: "DelegateUser",
     nullable: false,
     defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


        }
    }
}
