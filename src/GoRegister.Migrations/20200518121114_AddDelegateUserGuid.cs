using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddDelegateUserGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UniqueIdentifier",
                table: "DelegateUser",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "23005f8b-93ad-41fa-8054-212597b35a0c");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b5842d3b-520b-4e08-aef7-d327c123282d", "AQAAAAEAACcQAAAAEHIufU2WqBtjNu8vWGmN9Cu4S71KnQlkieZKFV+Cg0DmH91hjuRSzOEVZ1uGSSF0Tg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "de85d430-c255-4e09-a55e-cbada91a3206");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bb89608c-082d-446c-a469-93fa9405f797", "AQAAAAEAACcQAAAAEOnC4NJ2yI/5L4eenBRt8Ozv4mMM/QYvBHdyWg1Mfe+F2TqA+fHjsNE4Ho+2edv0aA==" });
        }
    }
}
