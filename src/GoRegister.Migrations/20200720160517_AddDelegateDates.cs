using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddDelegateDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledUtc",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedUtc",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedUtc",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvitedUtc",
                table: "DelegateUser",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledUtc",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "ConfirmedUtc",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "DeclinedUtc",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "InvitedUtc",
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
