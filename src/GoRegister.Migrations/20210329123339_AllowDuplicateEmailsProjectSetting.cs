using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AllowDuplicateEmailsProjectSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowDuplicateEmails",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEHBpZZCNrhICFRI/FEpIcPNiS6wl4+Gr4T/q6bYkucbIg8oxp+cGcBPqTQ5TEfwzKA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowDuplicateEmails",
                table: "Project");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEL0ub5W8qkTuAFZIIqO9Igj5zBJitBqjV5Ph5DAgYSNNtmxQyigeBgKM4XZzk05rmA==");
        }
    }
}
