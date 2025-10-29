using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MakeUserProjectNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "User",
                nullable: true,
                oldClrType: typeof(int));

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

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                columns: new[] { "NormalizedUserName", "ProjectId" },
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL AND [ProjectId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "User",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                columns: new[] { "NormalizedUserName", "ProjectId" },
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }
    }
}
