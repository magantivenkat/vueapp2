using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class DelegateModifiedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtc",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b675eb73-eb8f-4607-8455-8c5606c78691");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f7f2ca9a-e195-4563-9d3d-d2d2fa7f7df1", "AQAAAAEAACcQAAAAEN18lJbTy+iquW3jZeOhTxo7Z9c1UNXOzeg1ZYQFHBcbk0SA33NQGY6J43VRNesDiw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedUtc",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5a5fe2ef-b10c-4fc1-91d2-def290e13101");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8f8a517c-1fca-43a2-b296-21e7825fbe7e", "AQAAAAEAACcQAAAAEMaFz9O2TnivU/1EQZoSLjDwN6ov4UvTU92O9ctLngv0dVT0LPS3X/YZsrROD6MTJg==" });
        }
    }
}
