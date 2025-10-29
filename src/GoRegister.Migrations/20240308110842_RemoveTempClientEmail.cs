using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoRegister.Migrations
{
    public partial class RemoveTempClientEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempClientEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempClientEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempClientEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempClientEmail_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempClientEmail_ClientId",
                table: "TempClientEmail",
                column: "ClientId");
        }
    }
}
