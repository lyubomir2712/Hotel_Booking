using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Services.ViewModels;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HotelBooking.Web.Controllers
{
    public class Cart : Controller
    {
        private readonly BookingDbContext _dbContext;
        private readonly UserManager<UserModel> _userManager;

        public Cart(BookingDbContext dbContext, UserManager<UserModel> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        [HttpPost]
        public IActionResult AddToCart(string HotelName, string HotelImg, string HotelPrice, string StartAt, string EndAt) 
        {
            
            int UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            
            HotelModel newHotel = new HotelModel { HotelName = HotelName, HotelImg = HotelImg };

            BookingModel newBookingModel = new BookingModel
            {
                StartAt = Convert.ToDateTime(StartAt),
                Price = Convert.ToDouble(HotelPrice),
                EndAt = Convert.ToDateTime(EndAt),
                HotelModel = newHotel
            };


            UserBookingModel newUserBookingModel = new UserBookingModel
            {
                BookingModel = newBookingModel,
                UserId = UserId
        };

            _dbContext.Hotels.Add(newHotel);
            _dbContext.Bookings.Add(newBookingModel);
            _dbContext.UserBookings.Add(newUserBookingModel);
            _dbContext.SaveChanges();


            return RedirectToAction("Index", "Home");

        }
    }
}



