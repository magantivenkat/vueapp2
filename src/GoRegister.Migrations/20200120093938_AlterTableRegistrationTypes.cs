using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableRegistrationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RegistrationType",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "RegistrationType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RegistrationType",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "RegistrationType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "eb3098a6-5623-481e-a28e-1108da02303f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c72cf7af-61c0-4652-8ac8-d7054ba026ac", "AQAAAAEAACcQAAAAEPOZPq2BSDKcrUYO+AQ2LLTJTF1FIs8Pc7JFHgBqSi2lyPu7fVUzNhGxUmn2zIPegg==" });
        }
    }
}
