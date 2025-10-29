using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ConcurrencyStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a72c94f8-3167-493b-bf93-667efb4353c6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0d4b0423-891a-49ec-bef7-f3ff58f2c6a4", "AQAAAAEAACcQAAAAEO+kOCvElVIxX6rfQ1nZ+aMcPWUprLanDckRzbQ5jbKKJs4IaNBBqRTquvcnEMjMhQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "df9ada12-4d63-42cd-b57e-969e02028e53");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b24f0210-bf37-4083-8793-80f969f7f062", "AQAAAAEAACcQAAAAEFTifbovO0E5gd73LhkBJxJJBJ8Nvqj4cCuVmBIl+QBYqyKcMYk2kFXae5vCEH0F1Q==" });
        }
    }
}
