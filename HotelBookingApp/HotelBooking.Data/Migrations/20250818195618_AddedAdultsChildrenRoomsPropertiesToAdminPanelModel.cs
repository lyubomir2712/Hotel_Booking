using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class AddedAdultsChildrenRoomsPropertiesToAdminPanelModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdultsNumber",
                table: "AdminPanelBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildrenNumber",
                table: "AdminPanelBookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomsNumber",
                table: "AdminPanelBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdultsNumber",
                table: "AdminPanelBookings");

            migrationBuilder.DropColumn(
                name: "ChildrenNumber",
                table: "AdminPanelBookings");

            migrationBuilder.DropColumn(
                name: "RoomsNumber",
                table: "AdminPanelBookings");

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
    }
}
