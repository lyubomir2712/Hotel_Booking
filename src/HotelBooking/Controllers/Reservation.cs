using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    
    public class Reservation : Controller
    {
        public IActionResult ProcessReservation()
        {
            return View();
        }
    }
}
