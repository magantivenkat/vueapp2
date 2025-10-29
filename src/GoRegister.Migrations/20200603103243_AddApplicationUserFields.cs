using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddApplicationUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FieldType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 7, "FirstName" },
                    { 6, "Email" },
                    { 8, "LastName" }
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FieldType",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4c7579fe-b23b-4579-a8f2-381b60610a7f");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "47b64c4b-5343-4015-8b0e-9a0ccc2fd328", "AQAAAAEAACcQAAAAEEpv+WbtG55RJNq+AtSYgq6evIhikncIT7c+7HC6l7G7zMUrdFi+KlLZQDUWTEc6tA==" });
        }
    }
}
