using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTenantUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantUrl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Host = table.Column<string>(nullable: true),
                    IsSubdomainHost = table.Column<bool>(nullable: false),
                    DisallowedSubDomains = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantUrl_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "03833cb0-9f35-42f6-acd1-46ef116824db");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "35660517-67c0-4fcb-ac89-2a76ef299f5e", "AQAAAAEAACcQAAAAEA2JyL60cH1iJP+A0n6Yj64t6Hsn5XnyyqxzmfQ3tlTHhssS+L3dLCE023kr9AVxqg==" });

            migrationBuilder.CreateIndex(
                name: "IX_TenantUrl_ClientId",
                table: "TenantUrl",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantUrl");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "02102bf6-62a3-4f85-95fb-91712e8e100f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1d643ad8-fd8f-41b0-b64a-aa642f4b95fd", "AQAAAAEAACcQAAAAEPxat67Bm5bXZJQXU9SimVkuZttu9/ENgj+gXrR91asiaSxfJuD11hvIQu7nUrWSpA==" });
        }
    }
}
