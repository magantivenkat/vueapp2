using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterTableProjectPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DELETE FROM dbo.CustomPage");

            //migrationBuilder.DropIndex(
            //    name: "IX_RegistrationPage_ProjectPageId",
            //    table: "RegistrationPage");

            //migrationBuilder.DropIndex(
            //    name: "IX_CustomPage_ProjectPageId",
            //    table: "CustomPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "663708a8-a256-4cc4-aa04-f2f39e0feae6");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1fbca0a0-403e-47b3-834d-6d45b8a5236d", "AQAAAAEAACcQAAAAEKnBl8mIaPiD0WCyPOzHXOsUf3ylkFx9WueT5t6+fac+sL0V3CYsB+BvwBntDlv06g==" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_RegistrationPage_ProjectPageId",
            //    table: "RegistrationPage",
            //    column: "ProjectPageId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_CustomPage_ProjectPageId",
            //    table: "CustomPage",
            //    column: "ProjectPageId",
            //    unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegistrationPage_ProjectPageId",
                table: "RegistrationPage");

            migrationBuilder.DropIndex(
                name: "IX_CustomPage_ProjectPageId",
                table: "CustomPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5c7050f9-26a3-4676-90e5-9e4b5d2d75f4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3df42966-6f9e-422c-99d0-dddaf0a75558", "AQAAAAEAACcQAAAAEFXiKHMjIoVwcUU/AXo8+uKPql7DIxP6xwqlglWgR/dhnPoeC8phh5KKPTK4MgtEoA==" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPage_ProjectPageId",
                table: "RegistrationPage",
                column: "ProjectPageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPage_ProjectPageId",
                table: "CustomPage",
                column: "ProjectPageId");
        }
    }
}
