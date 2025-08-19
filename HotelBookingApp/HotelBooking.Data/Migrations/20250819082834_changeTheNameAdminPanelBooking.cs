using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class changeTheNameAdminPanelBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE2H2mP6C5g/Cnl/qFQNTpS6sIluJDqIK9IWS2AtHewuhTpJ7345+oGarfS4XKfNFg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA+YH6SDK4w4IDTRSEOWRqXN+PYw0uJoFtI2lHnRj9wbt28EHXtgeSPS9RC7WTGr6g==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKettpVK/JFH4LjUFUCWlqaU3F1Bk1+sC/iYE98VU1QPFHth4gkheeI3QNKTnDKMrg==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJuqAyrWkvEJFDWmtJ9vOe0Z3bAUCZ1v5x5Tl/01SgFM7NaRLwRVkW1AWBb3Ag2POQ==");
        }
    }
}
