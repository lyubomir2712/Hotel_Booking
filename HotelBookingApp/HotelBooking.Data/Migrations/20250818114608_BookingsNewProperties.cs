using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class BookingsNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AdultsNumber",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildrenNumber",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ReviewScore",
                table: "Bookings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewScoreWord",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEt588Srl8C80DZ2XHNPs3Lc9MvMgV5Hq8UsTVe8eZzt1YtN69tI4NTVqx0yPrcnmw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJneP4DLYe9Cc7gQsXWRd4IF2qPdpPPBSVr6EIXL1YnxWmNL1dSx5aE/tTyVAszCkQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AdultsNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ChildrenNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReviewScore",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReviewScoreWord",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDG1q2ebIhfYF5GEXKVsL84rXz5Vr5OyHpPXz5sIGBgQs9qteld+pagxpIrviXwzMw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECTR60KpGFW/2sh4q2libBiygAr7Ld7qLToixqwRVutkKWGpa74VdVobbWAJ10l8CA==");
        }
    }
}
