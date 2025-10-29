using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class MakeProjectIdNullableForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "ProjectId", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAEDCFdFmZV5ZjuwgZEHA45TbAGxIia4t3492cE9W4TH5EFvF7sCCDqwNQKmH6m1xq8w==", null, "d810c323-217a-45fd-a5a8-8eba550e85ad" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "ProjectId", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAEHBpZZCNrhICFRI/FEpIcPNiS6wl4+Gr4T/q6bYkucbIg8oxp+cGcBPqTQ5TEfwzKA==", 1, "" });
        }
    }
}
