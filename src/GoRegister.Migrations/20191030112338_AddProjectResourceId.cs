using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddProjectResourceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectResourceId",
                table: "Project",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "633f2b07-95cd-49b1-b2d4-19dd20506c4d");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f6087421-81ae-4ee5-8971-50f6882caf53", "AQAAAAEAACcQAAAAEKbETs82F9GT8mPRPfQMFUeO4BiTz+lwFDh0UbjC6fZFVK+LPGfnASQlkqXsCVa4Iw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectResourceId",
                table: "Project");

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
    }
}
