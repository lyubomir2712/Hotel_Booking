using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class AddedRoomsNumberPropertyToBookingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomsNumber",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHQOrKSQJo2PudFn8Fj46D+zLBaO+w5Z8o0W+/yk7PeH9pZmbQgSEl4dQzJygVC3sg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDk0bcTS8PdDRfwRn4sCFNVgQMrGlTenMa06D3Lqj+vAsmh3WNo++n7LhNK84X+6Hg==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomsNumber",
                table: "Bookings");

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
    }
}
