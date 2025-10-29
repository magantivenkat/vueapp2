using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class delegateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMYrJ5UIwMW0xbP+TyZZTzBcTf0L6FPv+91Lg7M4kuvS6rWVQvfl5uyQcE00OZAs4A==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEDsxFVsEyj5wpK4XE/TRj4cgHm5ykkR02oMnhIakdFpW7JXDi3DMUSmW0Fy3tAOZ/w==");
        }
    }
}
