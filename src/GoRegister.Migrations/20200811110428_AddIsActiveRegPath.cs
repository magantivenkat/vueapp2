using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddIsActiveRegPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTesting",
                table: "RegistrationPath",
                newName: "IsActive");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f0e67153-576a-4bf2-819e-8ee85cc55086");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "772aee8c-cb99-456a-bc59-1208d8a827b3", "AQAAAAEAACcQAAAAEEs5QupPZae3CquXu1CRgsZUeAecU0xzGFaVRC+O/aRw9HM9A4oj53eEOcqcXmv50Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "RegistrationPath",
                newName: "IsTesting");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "224ce477-b971-48be-9f69-ca66b0961d66");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "96a7b83e-3ecb-484d-bd1d-6e92e6236801", "AQAAAAEAACcQAAAAEN+e3WqUfd0PpkKWnY1k50DRX2QtWWs4pTKPAc8lHyOSaViR3dJKnjMePn0pJg2lYQ==" });
        }
    }
}
