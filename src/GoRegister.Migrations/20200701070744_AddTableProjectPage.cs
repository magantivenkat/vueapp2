using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddTableProjectPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ProjectPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    MenuPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.AddColumn<int>(
            //    name: "ProjectPageId",
            //    table: "RegistrationPage",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectPageId",
                table: "CustomPage",
                nullable: false,
                defaultValue: 0);

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

            //migrationBuilder.CreateIndex(
            //    name: "IX_RegistrationPage_ProjectPageId",
            //    table: "RegistrationPage",
            //    column: "ProjectPageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPage_ProjectPageId",
                table: "CustomPage",
                column: "ProjectPageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPage_ProjectId",
                table: "ProjectPage",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPage_ProjectPage_ProjectPageId",
                table: "CustomPage",
                column: "ProjectPageId",
                principalTable: "ProjectPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_RegistrationPage_ProjectPage_ProjectPageId",
            //    table: "RegistrationPage",
            //    column: "ProjectPageId",
            //    principalTable: "ProjectPage",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomPage_ProjectPage_ProjectPageId",
                table: "CustomPage");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_RegistrationPage_ProjectPage_ProjectPageId",
            //    table: "RegistrationPage");

            migrationBuilder.DropTable(
                name: "ProjectPage");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationPage_ProjectPageId",
                table: "RegistrationPage");

            migrationBuilder.DropIndex(
                name: "IX_CustomPage_ProjectPageId",
                table: "CustomPage");

            //migrationBuilder.DropColumn(
            //    name: "ProjectPageId",
            //    table: "RegistrationPage");

            migrationBuilder.DropColumn(
                name: "ProjectPageId",
                table: "CustomPage");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "04c54086-342f-428f-ab0a-f179f08b70ea");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2bd417db-b087-410f-8924-92e2e35344cd", "AQAAAAEAACcQAAAAENRQlQGvXJROPAIXmAUa2FePi75g5PabneEFpln7Su3aDZjfbMGqPJbA37nATi1bRg==" });
        }
    }
}
