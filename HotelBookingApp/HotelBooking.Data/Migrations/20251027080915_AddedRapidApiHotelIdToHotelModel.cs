using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRapidApiHotelIdToHotelModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RapidApiHotelId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RapidApiHotelId",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKS6mlLukoJmtUx9G+UcE+akx1R3zWCfSw2cQdL6eMXs5kN4ckZktk+Xmhleb6iOBg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC9SFhqLO/Idu9jBUbx8RnUnv2dwC8B10ix83N5kusjFPLVrGtfNf3SkdO2dntCYQQ==");
        }
    }
}
