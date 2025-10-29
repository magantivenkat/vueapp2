using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class RemoveFieldTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_FieldType_FieldTypeId",
                table: "Field");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropIndex(
                name: "IX_Field_FieldTypeId",
                table: "Field");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "041eca7a-8000-430b-82df-33acc57c70ec");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8fc8adb3-b935-4571-9d9b-b47531c89d6e", "AQAAAAEAACcQAAAAEB9ke4qZaJeLbQ7qju8LsQI9Jhlcyj8/lpe5E4KWAH0/Zy5JBQlAhqwprmCXpNRPAQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FieldType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Textbox" },
                    { 10, "PhoneNumber" },
                    { 2, "RadioButton" },
                    { 4, "TextArea" },
                    { 5, "Date" },
                    { 7, "FirstName" },
                    { 6, "Email" },
                    { 8, "LastName" }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b95d7183-c3b9-465f-98c8-3523bffee73a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a7511e79-4868-43cd-b483-1e6c437b56c4", "AQAAAAEAACcQAAAAECfiSPTPkSfxwZtFbsmx7IYlWW1s0090JtuqHnz8F2GH8ra7RrCU/DPMBaD3iXPpCQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldTypeId",
                table: "Field",
                column: "FieldTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_FieldType_FieldTypeId",
                table: "Field",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
