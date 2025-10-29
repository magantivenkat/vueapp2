using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddBatchIdToTableEmailAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                table: "EmailAudit",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c7db1e90-7668-48f7-8a51-115f5f02014a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7eac7566-64bc-4de5-a18d-72fde63f0db4", "AQAAAAEAACcQAAAAECljSu1UZjcLDZBk8YSxAHcOEE8fMKYcLP8+uJKNYygUvKH2+dTqqzQ1/ZIkCkk4Wg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "EmailAudit");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "24ee5723-bfe5-4a80-b1c5-44cd4a28c863");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d3d90f68-83ec-42e9-ba96-4e25b4226f94", "AQAAAAEAACcQAAAAEMMPJQWdPD01FHEaIJ0rCCaAB5mcHUyn6/Xip2cLEMhCZMd1iXwiVMfK60f53Y751Q==" });
        }
    }
}
