using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterCustomPageTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PageStatus",
                table: "CustomPageAudit",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CustomPageId",
                table: "CustomPageAudit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "eb19ec05-dd4c-45fc-b4da-c0edde22ae19");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5cc102c4-4a88-44cd-80e9-3d4aaec4c742", "AQAAAAEAACcQAAAAEHsVPB5QnS6L2LAm5Hr3ru4M6GUeR7C9BOk3gLScA8kZuTXEiPxnDde6C5F6PA1OjA==" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomPageAudit_CustomPageId",
                table: "CustomPageAudit",
                column: "CustomPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomPageAudit_CustomPage_CustomPageId",
                table: "CustomPageAudit",
                column: "CustomPageId",
                principalTable: "CustomPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomPageAudit_CustomPage_CustomPageId",
                table: "CustomPageAudit");

            migrationBuilder.DropIndex(
                name: "IX_CustomPageAudit_CustomPageId",
                table: "CustomPageAudit");

            migrationBuilder.DropColumn(
                name: "CustomPageId",
                table: "CustomPageAudit");

            migrationBuilder.AlterColumn<int>(
                name: "PageStatus",
                table: "CustomPageAudit",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c2cfa7e4-7e0a-41ea-9999-ca1358e5ebb5");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e66572a8-1aa6-4665-a91c-eee307976b67", "AQAAAAEAACcQAAAAEND2zxE7GNC36jrEG7fInSv77w7wHElRMjUxLMqlH4VCxlsNO7xJDO2QCfwDeRjBFw==" });
        }
    }
}
