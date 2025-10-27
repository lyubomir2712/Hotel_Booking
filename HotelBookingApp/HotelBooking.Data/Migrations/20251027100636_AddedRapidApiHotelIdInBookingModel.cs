using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRapidApiHotelIdInBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RapidApiHotelId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGVhkqugAYmJIAPMyVAdPUCRVdBp55PZKhEOF+Zs4WzRcQVxEN469S9HPtXgdMX6fg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA5J4qGTeejATZLy6kmZB4b91CxcEOduw7y4wUQl05ek/23392vwSEXY5HoZyDwnSg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RapidApiHotelId",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL1oeest6LVpbRpbuzA8jvnB45QwPSi9+YvmtRzt/S0i4LM5T/f9+XdMHIrRl3uWIQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELuTBQpBXmKX6da2ePBQE59gDNUU/7IMUt9O7eQQDaLSMkVV67wVcwXi4tmpfgNYMQ==");
        }
    }
}
