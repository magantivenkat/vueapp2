using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class UpdateThemesWithOverrideCSS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlExec(@"
                update ProjectTheme 
                set ThemeCss = ISNULL(ThemeCss, '') + ' ' + ISNULL(OverrideCss, '')
                where ProjectId is not null and OverrideCss is not null
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
