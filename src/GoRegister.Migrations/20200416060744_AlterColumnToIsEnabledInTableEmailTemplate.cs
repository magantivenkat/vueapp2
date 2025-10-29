using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterColumnToIsEnabledInTableEmailTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "622101bc-ab81-4921-8e1f-dc8483782093");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b8f5a999-3626-42c2-9101-b0f8026aca99", "AQAAAAEAACcQAAAAEByg14IEEUdDj7HysPRF+qDp3JC1ESqUR1cjznwnRGTnabekV4vpFAgPsVb7PtVMqw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
