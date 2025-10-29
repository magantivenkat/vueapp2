using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTPNReportDataStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPNReportDataStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TPNReportDetailsId = table.Column<int>(nullable: false),
                    MRFClientResponseDetailsId = table.Column<int>(nullable: false),
                    IsSendWeekly = table.Column<bool>(nullable: false),
                    IsSendFortNightly = table.Column<bool>(nullable: false),
                    IsSendMonthly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPNReportDataStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPNReportDataStatus");
        }
    }
}
