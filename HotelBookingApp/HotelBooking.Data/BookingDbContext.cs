using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data
{
    public class BookingDbContext : IdentityDbContext<UserModel, UserRole, int>
    {
        //public BookingDbContext()
        //{

        //}
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
             base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=(Local)\\SQLEXPRESS01;Database=aspnet-HotelBooking;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;");
            //   base.OnConfiguring(optionsBuilder);
        }
    }
}
