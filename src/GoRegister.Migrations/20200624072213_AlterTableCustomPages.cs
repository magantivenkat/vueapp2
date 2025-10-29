using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableCustomPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "CustomPage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "04c54086-342f-428f-ab0a-f179f08b70ea");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2bd417db-b087-410f-8924-92e2e35344cd", "AQAAAAEAACcQAAAAENRQlQGvXJROPAIXmAUa2FePi75g5PabneEFpln7Su3aDZjfbMGqPJbA37nATi1bRg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "CustomPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bae1fa16-a61b-4587-8e86-cbb47d5f16eb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4a839f6-e8bb-42d4-9f96-f5a5a4ce2b75", "AQAAAAEAACcQAAAAEOFETd3+aMKb5aITmb089MIYcLdqY3+q6WhW1/9UeCjayIE+CB7D3uuI24iUgABHFw==" });
        }
    }
}
