using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableEmailTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "EmailTemplate",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "EmailTemplate");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0639693f-8ab2-4387-94fa-38336e9c3bf9");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6d22f8c2-0757-4542-9ccf-b8d60ca4abf2", "AQAAAAEAACcQAAAAEHOkbGTRxJrzrQJGvUg/PZ39dJ2gKDuI28cuxMyJN9bK3MoFaesXParYtVE8Fw5/Cw==" });
        }
    }
}
