using HotelBooking.Models.AppModels;
using HotelBooking.Models.BaseModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<AdminPanelBookings> AdminPanelBookings { get; set; }

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

         

            builder.Entity<UserBookingModel>()
                .HasOne(ub => ub.UserModel)
                .WithMany(u => u.UserBookingModels)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                

            builder.Entity<BookingModel>()
                .HasOne(b => b.HotelModel)
                .WithMany(h => h.BookingModels)
                .HasForeignKey(b => b.Id)
                .OnDelete(DeleteBehavior.NoAction);
            
            
            builder.Entity<AdminPanelBookings>()
                .HasOne(apb => apb.HotelModel)
                .WithMany(h => h.AdminPanelBookings)
                .HasForeignKey(apb => apb.HotelModelId)
                .OnDelete(DeleteBehavior.NoAction);

            // Create a password hasher
            var hasher = new PasswordHasher<UserModel>();

            // Seed users
            var adminUser = new UserModel
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@hotel.com",
                NormalizedEmail = "ADMIN@HOTEL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                FirstName = "System",
                LastName  = "Administrator",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var user1 = new UserModel
            {
                Id = 2,
                UserName = "john",
                NormalizedUserName = "JOHN",
                Email = "john@example.com",
                NormalizedEmail = "JOHN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "User123!"),
                FirstName = "John",
                LastName  = "Doe",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var user2 = new UserModel
            {
                Id = 3,
                UserName = "jane",
                NormalizedUserName = "JANE",
                Email = "jane@example.com",
                NormalizedEmail = "JANE@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "User123!"),
                FirstName = "Jane",
                LastName  = "Doe",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            
            

            builder.Entity<UserModel>().HasData(adminUser, user1, user2);

            // Assign users to roles
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 }, // Admin
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // Regular
                new IdentityUserRole<int> { UserId = 3, RoleId = 2 }  // Regular
            );

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
