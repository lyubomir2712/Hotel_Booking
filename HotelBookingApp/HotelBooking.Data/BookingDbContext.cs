using HotelBooking.Models.AppModels;
using HotelBooking.Models.BaseModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
