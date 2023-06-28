using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.HotelAddService;
using HotelBooking.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.Web.Controllers
{
    public class BookedHotelsByUserController : Controller
    {
        readonly HotelAddService _hotelAddService;
        public BookedHotelsByUserController(HotelAddService hotelAddService) 
        {
            _hotelAddService= hotelAddService;
            
        }

        
        public IActionResult BookedHotels()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<BookingModel> bookings = _hotelAddService.GetBookedHotels(userId);
            List<HotelModel> hotels = _hotelAddService.GetHotels(bookings);

            UserBookedHotels userBookedHotels = new UserBookedHotels 
            {
                Hotels = hotels,
                Bookings = bookings
            };
                

            
            
            return View("~/Views/Home/BookedHotels.cshtml", userBookedHotels);

        }
    }
}
