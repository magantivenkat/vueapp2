using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddJoinTableCustomPageRegistrationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "DateFormat",
            //    table: "User",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "TimeZone",
            //    table: "User",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomPageRegistrationType",
                columns: table => new
                {
                    CustomPageId = table.Column<int>(nullable: false),
                    RegistrationTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPageRegistrationType", x => new { x.CustomPageId, x.RegistrationTypeId });
                    table.ForeignKey(
                        name: "FK_CustomPageRegistrationType_CustomPage_CustomPageId",
                        column: x => x.CustomPageId,
                        principalTable: "CustomPage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomPageRegistrationType_RegistrationType_RegistrationTypeId",
                        column: x => x.RegistrationTypeId,
                        principalTable: "RegistrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "37ec22f2-3f4b-48a9-980f-c3ef020cfe1b");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3cd55b93-62c9-4e04-94a2-a6ec626c1d2b", "AQAAAAEAACcQAAAAEJ9pJMG9BpyH1nO30QVOia2TqfDX8OJ9gg2OZAVnzwY8BZRgT6WbxevdvOQyglX7tA==" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageRegistrationType_RegistrationTypeId",
                table: "CustomPageRegistrationType",
                column: "RegistrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "CustomPageRegistrationType");

            //migrationBuilder.DropColumn(
            //    name: "DateFormat",
            //    table: "User");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "User");

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
