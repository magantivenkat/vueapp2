using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AlterRegistrationTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationType_InvitationListId",
                table: "RegistrationType");

            migrationBuilder.DropColumn(
                name: "InvitationListId",
                table: "RegistrationType");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d3019239-e7cf-4709-aeea-3a9e15b83ea3");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3c9f7edb-de95-4d4a-83e2-74a12aa2a35e", "AQAAAAEAACcQAAAAENBONY3CLzPcGhrxVXm1bNUnkN4/flmc5JKH+VHG0ccdscW7m8SkqbKLdotDwxY50A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationListId",
                table: "RegistrationType",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d59e5010-7b6d-4ff1-b70b-8f21e26c1650");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "777c8614-b042-4784-a182-dd1e307b2af3", "AQAAAAEAACcQAAAAENv8tfvqL4eNd9wX/KiD7r6GqKqZivOfqIvj1tTTaqRQnuPQBBkDgODEB+IBpujqdA==" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationType_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationType_InvitationList_InvitationListId",
                table: "RegistrationType",
                column: "InvitationListId",
                principalTable: "InvitationList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
