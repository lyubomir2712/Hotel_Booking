using HotelBooking.Models.AppModels;
using HotelBooking.Models.BaseModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data
{
    public class BookingDbContext : IdentityDbContext<UserModel, UserRole, int>
    {
        
        public DbSet<BookingModel>? Bookings { get; set; }
        public DbSet<HotelModel>? Hotels { get; set; }
        public DbSet<UserBookingModel>? UserBookings { get; set; }

        public DbSet<AdminPanelBooking> AdminPanelBookings { get; set; }

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasData(
                new UserRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = 1
                },
                new UserRole()
                {
                    Name = "Regular",
                    NormalizedName="REGULAR",
                    Id = 2,
                }
            );

            // Seed default users
            var hasher = new PasswordHasher<UserModel>();

            var adminUser = new UserModel
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@yahoo.com",
                NormalizedUserName = "ADMIN@YAHOO.COM",
                Email = "admin@yahoo.com",
                NormalizedEmail = "ADMIN@YAHOO.COM",
                EmailConfirmed = true,
                SecurityStamp = "f3e3d8b1-9c9c-4a8b-9e6f-5e5c67890a12",
                ConcurrencyStamp = "7b6f0c8a-15c4-4a6e-9a03-ff0d3f6d1d7a"
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123");

            var regularUser = new UserModel
            {
                Id = 2,
                FirstName = "Lyubomir",
                LastName = "Georgiev",
                UserName = "lyubomir@gmail.com",
                NormalizedUserName = "LYUBOMIR@GMAIL.COM",
                Email = "lyubomir@gmail.com",
                NormalizedEmail = "LYUBOMIR@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = "2a4b01f9-3e5d-4f53-ae4e-3a8b9c2d7f5e",
                ConcurrencyStamp = "b7f4e9a2-1b2c-4e5f-9a1b-3c4d5e6f7a8b"
            };
            regularUser.PasswordHash = hasher.HashPassword(regularUser, "password123");

            builder.Entity<UserModel>().HasData(adminUser, regularUser);

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }
            );

         

            builder.Entity<UserBookingModel>()
                .HasOne(ub => ub.UserModel)
                .WithMany(u => u.UserBookingModels)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                

            builder.Entity<BookingModel>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();              

            builder.Entity<BookingModel>()
                .HasOne(b => b.HotelModel)
                .WithMany(h => h.BookingModels)
                .HasForeignKey(b => b.HotelModelId)  
                .OnDelete(DeleteBehavior.NoAction);
            
            
            builder.Entity<AdminPanelBooking>()
                .HasOne(apb => apb.HotelModel)
                .WithMany(h => h.AdminPanelBookings)
                .HasForeignKey(apb => apb.HotelModelId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
