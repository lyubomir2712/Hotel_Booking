using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    public partial class AdminAndUserSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "7b6f0c8a-15c4-4a6e-9a03-ff0d3f6d1d7a", "admin@yahoo.com", true, "Admin", "Admin", false, null, "ADMIN@YAHOO.COM", "ADMIN@YAHOO.COM", "AQAAAAIAAYagAAAAEDG1q2ebIhfYF5GEXKVsL84rXz5Vr5OyHpPXz5sIGBgQs9qteld+pagxpIrviXwzMw==", null, false, "f3e3d8b1-9c9c-4a8b-9e6f-5e5c67890a12", false, "admin@yahoo.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 2, 0, "b7f4e9a2-1b2c-4e5f-9a1b-3c4d5e6f7a8b", "lyubomir@gmail.com", true, "Lyubomir", "Georgiev", false, null, "LYUBOMIR@GMAIL.COM", "LYUBOMIR@GMAIL.COM", "AQAAAAIAAYagAAAAECTR60KpGFW/2sh4q2libBiygAr7Ld7qLToixqwRVutkKWGpa74VdVobbWAJ10l8CA==", null, false, "2a4b01f9-3e5d-4f53-ae4e-3a8b9c2d7f5e", false, "lyubomir@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
