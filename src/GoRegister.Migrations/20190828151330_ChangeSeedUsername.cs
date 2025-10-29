using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ChangeSeedUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "20908698-6a69-4f6a-a57d-0a803c2dfc97", "WEBMASTER@BANKS-SADLER.COM", "AQAAAAEAACcQAAAAEAjr9b2lDiKACSLXtyXdPjGlwIPHvTdkghWNpmlUIa6sF8jFs9B25SHeyEY02kB1Cg==", "WEBMASTER@BANKS-SADLER.COM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2f58661b-8d06-44ec-9a62-cdb0971d2399");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "213294c5-4051-49b4-a927-5af2aedabe86", "admin", "AQAAAAEAACcQAAAAEDOEwKGNv/GgdFeL6ZalKL5JFn2BYCSY2467rhffD1Y4X/2KFt2n8n2N2osF75KPjQ==", "admin" });
        }
    }
}
