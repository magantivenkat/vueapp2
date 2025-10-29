using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class ReportNameAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DataQuery",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DataQuery",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEIMhyi8blPVLieYUVS22FRSc7xS4HnjCjDgAKtw2i0VG7ULqHII1GRacLuK4Jj9O3w==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DataQuery");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DataQuery");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEGA0SzmuaAXoR38ae/OfWfl6rDBSi1dKaDnsVqWPF/n2OsQwIp37IVodXf8UstoGJA==");
        }
    }
}
