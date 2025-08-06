using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace HotelBooking.Web.Controllers
{
    public class RemoveHotelFromCartController : Controller
    {
        private readonly BookingDbContext _bookingDbContext;
        public RemoveHotelFromCartController(BookingDbContext bookingDbContext) 
        {
            _bookingDbContext = bookingDbContext;
        }
        public IActionResult RemoveHotel(int BookingId)
        {
            _bookingDbContext.Bookings.Remove(_bookingDbContext.Bookings.First(b=>b.Id == BookingId));
            _bookingDbContext.SaveChanges();
            return RedirectToAction("BookedHotels", "BookedHotelsByUser");
        }
    }
}
