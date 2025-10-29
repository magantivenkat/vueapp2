using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class PendingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d5530219-9060-4909-a630-9350211cad20");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e1230667-e26b-4ab2-8e48-e05db98ec187", "AQAAAAEAACcQAAAAELTM0RASLeQ9sxy9znc1TMWwtyYsGhWExy1HrzEtDXac3qkAm7r2qZPDqY7yRxrg6g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "13cdbe13-4f67-47fd-994c-011275ad9f09");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9c6af5ea-aa00-41f3-ba33-44c4196c15b0", "AQAAAAEAACcQAAAAELNMlYqntjsZEDWjXZW1hA0sOfPIebY+7GkS/YJtnzwUjUOr6VSrSw6q3/dPBrxzFw==" });
        }
    }
}
