using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class NextFieldForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptionRule_Field_FieldId",
                table: "FieldOptionRule");

            migrationBuilder.DropIndex(
                name: "IX_FieldOptionRule_FieldId",
                table: "FieldOptionRule");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "FieldOptionRule");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4f58402a-1951-4e2b-85cc-53ee0fbd2131");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f3b1c4b1-58eb-44d3-b4cc-e056497ecb41", "AQAAAAEAACcQAAAAEIKOFqiO4Y9dk/wLvG1+MQ/005ovDnPgJh7oJggxAhF59jXnkCMNSdYmUILxsTOATw==" });

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_NextFieldId",
                table: "FieldOptionRule",
                column: "NextFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptionRule_Field_NextFieldId",
                table: "FieldOptionRule",
                column: "NextFieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldOptionRule_Field_NextFieldId",
                table: "FieldOptionRule");

            migrationBuilder.DropIndex(
                name: "IX_FieldOptionRule_NextFieldId",
                table: "FieldOptionRule");

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "FieldOptionRule",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_FieldOptionRule_FieldId",
                table: "FieldOptionRule",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOptionRule_Field_FieldId",
                table: "FieldOptionRule",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
