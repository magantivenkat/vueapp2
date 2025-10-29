using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class CheckForChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f0a144a3-7eed-4977-8bc4-15119ac218eb");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "45966104-c6fe-4efb-9a00-654e1101b3e9", "AQAAAAEAACcQAAAAEIXIY1ac+N0ePXjJXDMcQhxg5uhNh1eWRiKZYe3WZqdXjWbNURGsDV2946rZFAbfMA==" });
        }
    }
}
