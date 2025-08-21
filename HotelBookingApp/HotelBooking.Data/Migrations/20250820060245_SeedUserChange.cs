using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class SeedUserChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Email", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "lyubomirbratov@gmail.com", "Bratov", "LYUBOMIRBRATOV@GMAIL.COM", "LYUBOMIRBRATOV@GMAIL.COM", "AQAAAAIAAYagAAAAEC9SFhqLO/Idu9jBUbx8RnUnv2dwC8B10ix83N5kusjFPLVrGtfNf3SkdO2dntCYQQ==", "lyubomirbratov@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAH2Su2XHn9sBNRAaGGnMeIbNsTNvIy+z5fvEzOCuqRSJLn0DdwhR7J44RalIQWVvw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "lyubomir@gmail.com", "Georgiev", "LYUBOMIR@GMAIL.COM", "LYUBOMIR@GMAIL.COM", "AQAAAAIAAYagAAAAEHyQaAecamzWlBLE6Aaaod1OqWHfuUaFfITyGz8VvZSrqVPC1Rta8jdEAGZbOE4Mhg==", "lyubomir@gmail.com" });
        }
    }
}
