using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTypeToTextField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputType",
                table: "Field",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "30070246-c935-4077-b278-bb6ccb917066");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e575f5f6-7b7a-4a21-9d4e-99324b45dd78", "AQAAAAEAACcQAAAAEHlVhxGjRz2ufcucke020spRwwKW59xW/qrIER0qtuzmgJ/7qL5PsDDAB11/4edCxQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputType",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b675eb73-eb8f-4607-8455-8c5606c78691");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f7f2ca9a-e195-4563-9d3d-d2d2fa7f7df1", "AQAAAAEAACcQAAAAEN18lJbTy+iquW3jZeOhTxo7Z9c1UNXOzeg1ZYQFHBcbk0SA33NQGY6J43VRNesDiw==" });
        }
    }
}
