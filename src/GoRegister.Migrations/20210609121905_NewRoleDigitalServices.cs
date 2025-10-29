using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class NewRoleDigitalServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "5c537e38-9b4d-4354-89da-fab33dd47c28", "DigitalServices", "DIGITALSERVICES" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAENMxgHDbWahiIVUJ45rbkDmV9buPnAWwi3zzr152bFJMjTXVbtSh6sTQA6jP6FfPgQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEFgk8PbTLtuqmVoNmbCjWCMjWfdGqUiFvQU3WKAUp6uo2LSU+0tCZpXaTPRdXBHJtA==");
        }
    }
}
