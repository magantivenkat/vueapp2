using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddIsLiveColumnToTableProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLive",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "72cb43e2-be4a-48c4-9dbd-2d98e89b739c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "da342415-a562-45eb-822f-9b9a5adf102e", "AQAAAAEAACcQAAAAEByZXAAfpqRy4BR6/y7seS9OXj88tCt8V/U5nD1lyvGdgNb+/HmmoKFUZ4zUPa7Qsw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLive",
                table: "Project");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "92ef2378-3eb3-4fb4-ad79-beed09bb2445");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "067cfe1e-0abc-461d-9eee-b7d8e057ae75", "AQAAAAEAACcQAAAAEOfhCKT+vRlaV4/cSaG5yOrOMRX3guXO4VqSPBsICiLfcXo2f+POzVA7UHizjFT97A==" });
        }
    }
}
