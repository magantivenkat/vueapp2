using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class FormAdminDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminDisplayName",
                table: "Form",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDNCGcc8SSjjhQC/nK5hkbySIdz0Nih+L1LhEsu5nA4dSa/eGjhcpOgZJLQOtFYRGg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminDisplayName",
                table: "Form");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEH63ep90osxjBu0kgssPiGmvEDsFBLxV6JbVQ1EYFCczsGh1Y5TK2+z+tFQlAj1OKw==");
        }
    }
}
