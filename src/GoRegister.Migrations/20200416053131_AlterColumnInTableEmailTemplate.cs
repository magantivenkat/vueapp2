using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterColumnInTableEmailTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "EmailTemplate",
                newName: "IsActivated");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2703620f-a49e-4e67-91f3-205a99d9a8b5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "24c046a6-10d0-4545-af3e-8508cfc7fadc", "AQAAAAEAACcQAAAAEBRpevPqaHjIQjgmYtbxw5bvKQtMe2xwJN7llKnQEPCn7URGMT+kmxTuVubpz77ABw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActivated",
                table: "EmailTemplate",
                newName: "IsEnabled");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a72c94f8-3167-493b-bf93-667efb4353c6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0d4b0423-891a-49ec-bef7-f3ff58f2c6a4", "AQAAAAEAACcQAAAAEO+kOCvElVIxX6rfQ1nZ+aMcPWUprLanDckRzbQ5jbKKJs4IaNBBqRTquvcnEMjMhQ==" });
        }
    }
}
