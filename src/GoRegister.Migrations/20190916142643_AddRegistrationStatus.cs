using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddRegistrationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFieldResponse_User_ApplicationUserId",
                table: "UserFieldResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserFieldResponse_ApplicationUserId",
                table: "UserFieldResponse");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserFieldResponse");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationStatusId",
                table: "DelegateUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RegistrationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RegistrationStatus",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 0, "Not Invited" },
                    { 1, "Invited" },
                    { 2, "Confirmed" },
                    { 3, "Declined" },
                    { 4, "Cancelled" },
                    { 5, "Waiting" }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fa6aaf52-ee6b-4f19-be46-5f479e6e092a");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2365f6c6-6b03-45a3-8ef6-4e47ce0c17fc", "AQAAAAEAACcQAAAAEPN1KruTDZ/NDxvyA+19imDXNihN1o7nJ2z/7CJFxEGT3z6VRGa7B2Vfhqw069w/VA==" });

            migrationBuilder.CreateIndex(
                name: "IX_DelegateUser_RegistrationStatusId",
                table: "DelegateUser",
                column: "RegistrationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DelegateUser_RegistrationStatus_RegistrationStatusId",
                table: "DelegateUser",
                column: "RegistrationStatusId",
                principalTable: "RegistrationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DelegateUser_RegistrationStatus_RegistrationStatusId",
                table: "DelegateUser");

            migrationBuilder.DropTable(
                name: "RegistrationStatus");

            migrationBuilder.DropIndex(
                name: "IX_DelegateUser_RegistrationStatusId",
                table: "DelegateUser");

            migrationBuilder.DropColumn(
                name: "RegistrationStatusId",
                table: "DelegateUser");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "UserFieldResponse",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d9a7825f-30bc-426b-89bf-57baf7a5e237");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ec921a65-174b-48bb-b17f-d39cf0698d0b", "AQAAAAEAACcQAAAAEOCuyUp73pfJt17i034EapgLMcNx+RFSoMjyAz7KDxrlnnWEVGoY48fHLrEiV2Sznw==" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFieldResponse_ApplicationUserId",
                table: "UserFieldResponse",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFieldResponse_User_ApplicationUserId",
                table: "UserFieldResponse",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
