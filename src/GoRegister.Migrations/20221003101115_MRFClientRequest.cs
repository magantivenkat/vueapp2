using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MRFClientRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MRFClientResponse",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MRFClientRequest",
                columns: table => new
                {
                    ClientuniqueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientUuid = table.Column<string>(nullable: true),
                    ClientID = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    MRFClientStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFClientRequest", x => x.ClientuniqueID);
                });

            migrationBuilder.CreateTable(
                name: "MRFClientRequestCountry",
                columns: table => new
                {
                    CountryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientuniqueID = table.Column<int>(nullable: false),
                    CountryName = table.Column<string>(nullable: true),
                    Countryguid = table.Column<string>(nullable: true),
                    MRFClientRequestClientuniqueID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRFClientRequestCountry", x => x.CountryID);
                    table.ForeignKey(
                        name: "FK_MRFClientRequestCountry_MRFClientRequest_MRFClientRequestClientuniqueID",
                        column: x => x.MRFClientRequestClientuniqueID,
                        principalTable: "MRFClientRequest",
                        principalColumn: "ClientuniqueID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MRFClientRequestCountry_MRFClientRequestClientuniqueID",
                table: "MRFClientRequestCountry",
                column: "MRFClientRequestClientuniqueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MRFClientRequestCountry");

            migrationBuilder.DropTable(
                name: "MRFClientRequest");

            migrationBuilder.DropColumn(
                name: "MRFClientResponse",
                table: "DelegateUser");
        }
    }
}
