using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddBccToTableEmailTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bcc",
                table: "EmailTemplate",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "EmailAudit",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e6f997b7-a43e-442b-8cb9-e471bb1d6b10");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7c92f9b1-0e9a-4e8c-8df3-c5f702a3970c", "AQAAAAEAACcQAAAAEKkGL3BVA/nIUWWRgWT/H+CipGmtT9yrEB5vedTe27JjCxjv1ekjtQD8LRTi7Ra8NA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bcc",
                table: "EmailTemplate");

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "EmailAudit",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

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
    }
}
