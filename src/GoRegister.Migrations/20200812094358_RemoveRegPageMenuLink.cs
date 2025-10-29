using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveRegPageMenuLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //commented out the migration that created this stuff because it isn't required.
            //leaving here in case it is needed for anyone who already applied broken migration

            //migrationBuilder.DropForeignKey(
            //    name: "FK_RegistrationPage_ProjectPage_ProjectPageId",
            //    table: "RegistrationPage");

            //migrationBuilder.DropIndex(
            //    name: "IX_RegistrationPage_ProjectPageId",
            //    table: "RegistrationPage");

            //migrationBuilder.DropColumn(
            //    name: "ProjectPageId",
            //    table: "RegistrationPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5a5fe2ef-b10c-4fc1-91d2-def290e13101");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8f8a517c-1fca-43a2-b296-21e7825fbe7e", "AQAAAAEAACcQAAAAEMaFz9O2TnivU/1EQZoSLjDwN6ov4UvTU92O9ctLngv0dVT0LPS3X/YZsrROD6MTJg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "ProjectPageId",
            //    table: "RegistrationPage",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f0e67153-576a-4bf2-819e-8ee85cc55086");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "772aee8c-cb99-456a-bc59-1208d8a827b3", "AQAAAAEAACcQAAAAEEs5QupPZae3CquXu1CRgsZUeAecU0xzGFaVRC+O/aRw9HM9A4oj53eEOcqcXmv50Q==" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_RegistrationPage_ProjectPageId",
            //    table: "RegistrationPage",
            //    column: "ProjectPageId",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_RegistrationPage_ProjectPage_ProjectPageId",
            //    table: "RegistrationPage",
            //    column: "ProjectPageId",
            //    principalTable: "ProjectPage",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
