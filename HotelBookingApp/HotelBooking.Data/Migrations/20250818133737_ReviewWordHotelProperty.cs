using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class ReviewWordHotelProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewScoreWord",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "ReviewScoreWord",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHLTKYLAF7HZqD9SVVVDTInvMUt+DCQZOhBMiQCrf9gyULMckWn55RaRmgVwDziqhw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ9w3Z2r3xdE/p6yxztqyl8FUrIGHqOfwLeot/cbFspVPuMCkNIR11pCKm/sdJoAng==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewScoreWord",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "ReviewScoreWord",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGQ6VtDYPfOIYQSka/dmYNFWacsqmV3rBd0AXJI3e0WUyBxpE19vOg5Clf0Uuh7qEA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBF1jIqF3NS8qxU7cCNt9wuXYjIznsltXX9qg5Tv6crgkJjiDxUvGPqf0x92qoQ9vg==");
        }
    }
}
