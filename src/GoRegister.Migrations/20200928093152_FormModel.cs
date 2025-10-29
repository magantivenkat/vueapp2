using GoRegister.ApplicationCore.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class FormModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "RegistrationPage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "Field",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    FormTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Form_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "71880ff1-fdb1-4240-87c8-bc22b59b8317");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3d341ca9-a111-4641-a680-235c789e55d7", "AQAAAAEAACcQAAAAEHG1t4dvJNq2U482/tsk/58Z9DnqCCfXf/iDcGJ++Af0MgUgMEq+5+jBEUtZsqtecw==" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationPage_FormId",
                table: "RegistrationPage",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FormId",
                table: "Field",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_ProjectId",
                table: "Form",
                column: "ProjectId");

            // update database with Form for each project
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.SqlExec(@"
                    DECLARE @idIterator int = (SELECT MIN(p.Id) FROM Project p)
                    DECLARE @idEnd int = (SELECT MAX(p.Id) FROM Project p) + 1

                    WHILE @idIterator < @idEnd
	                    BEGIN
	                    print(@idIterator)

	                    INSERT INTO dbo.Form
	                    (
	                        --Id - column value is auto-generated
	                        ProjectId,
		                    FormTypeId
	                    )
	                    VALUES
	                    (
	                        -- Id - int
		                    @idIterator,
		                    0
	                    )

	                    DECLARE @FormId int = SCOPE_IDENTITY()

	                    UPDATE dbo.Field
	                    SET
		                    FormId = @FormId
	                    WHERE ProjectId = @idIterator

	                    UPDATE dbo.RegistrationPage
	                    SET
		                    FormId = @FormId
	                    WHERE ProjectId = @idIterator


	                    SET @idIterator = 
	                    (
		                    SELECT TOP 1 p.Id FROM Project p
		                    WHERE p.Id > @idIterator
		                    ORDER BY p.Id ASC
	                    )
                    END");
            }

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationPage_Form_FormId",
                table: "RegistrationPage",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_FormId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationPage_Form_FormId",
                table: "RegistrationPage");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationPage_FormId",
                table: "RegistrationPage");

            migrationBuilder.DropIndex(
                name: "IX_Field_FormId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "RegistrationPage");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8b0f7608-b1de-4aad-b645-1daa1770e54e");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ddf4929-8068-4820-982f-2e7c7eade78b", "AQAAAAEAACcQAAAAEEJrvv/JF5go7RzIB3GIqsKsKog+wv2KIq3AUJrsXT8tgrzF9xiCKnWUow7BQv6SNw==" });
        }
    }
}
