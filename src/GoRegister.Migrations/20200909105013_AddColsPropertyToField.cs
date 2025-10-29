using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddColsPropertyToField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cols",
                table: "Field",
                nullable: false,
                defaultValue: 12);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cols",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "492417fd-8ce1-4f6e-a7b8-83da3f9bf574");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "238b73cb-14d2-4680-a611-2658bc17ca3b", "AQAAAAEAACcQAAAAEGSv12AJTYsieKWZtQkF1irDMQ8hCM7U/3035V6fsdjHQC7XipBOB/OxIQgQHd0tGA==" });
        }
    }
}
