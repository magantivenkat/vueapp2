using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddDateColumnsToTableDelegates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "DelegateUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "DelegateUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedDate",
                table: "DelegateUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitedDate",
                table: "DelegateUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "DeclinedDate",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "InvitedDate",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7e123ed7-fd23-4c7d-9f89-8f1fb18b1ff4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "041a5018-debd-4517-acc8-1bd74194daa8", "AQAAAAEAACcQAAAAEMW9xDrQctZDgVUKY7BtHNigw7eRYjlpnIqgqW/k9N9VY4gYHhy43Xrd0vQLFuN6Lw==" });
        }
    }
}
