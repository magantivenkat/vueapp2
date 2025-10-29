using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class UpdateToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d9a7825f-30bc-426b-89bf-57baf7a5e237");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ec921a65-174b-48bb-b17f-d39cf0698d0b", "AQAAAAEAACcQAAAAEOCuyUp73pfJt17i034EapgLMcNx+RFSoMjyAz7KDxrlnnWEVGoY48fHLrEiV2Sznw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d8086c85-5a84-498b-96f4-c09b4e1022b6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "20908698-6a69-4f6a-a57d-0a803c2dfc97", "AQAAAAEAACcQAAAAEAjr9b2lDiKACSLXtyXdPjGlwIPHvTdkghWNpmlUIa6sF8jFs9B25SHeyEY02kB1Cg==" });
        }
    }
}
