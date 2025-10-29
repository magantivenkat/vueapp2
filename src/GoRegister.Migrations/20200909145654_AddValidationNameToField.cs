using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddValidationNameToField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidationName",
                table: "Field",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8b0f7608-b1de-4aad-b645-1daa1770e54e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ddf4929-8068-4820-982f-2e7c7eade78b", "AQAAAAEAACcQAAAAEEJrvv/JF5go7RzIB3GIqsKsKog+wv2KIq3AUJrsXT8tgrzF9xiCKnWUow7BQv6SNw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationName",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b44a9f81-748e-41d9-b24e-f7710335dd4f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "67b28ef7-1d15-4b61-8dc7-98352ddd420d", "AQAAAAEAACcQAAAAEBFZ8fqt29jA4l7LRI3GqyG/mwvnh00GJCdaPQKHT32uP1NaqdFpn2R0vrRODgFfJg==" });
        }
    }
}
