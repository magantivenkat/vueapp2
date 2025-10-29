using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class CancellationFormProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReviewPageHidden",
                table: "Form",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SubmitButtonText",
                table: "Form",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationDocument",
                table: "DelegateUser",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEJvaGib8+LlH7SONDqmARSAO+xKobgMD1uq8dztNFrACnKzIcv8qr1IrRM2vY5vt/Q==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewPageHidden",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "SubmitButtonText",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "CancellationDocument",
                table: "DelegateUser");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMoky7mpLD5YYehLC6iQ2CTGrsGncQRBhicYnUThEWQ9+S0Ppzi83O1H2AeMBhAPmA==");
        }
    }
}
