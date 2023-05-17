using HotelBooking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Newtonsoft.Json.Linq;

namespace HotelBooking.Controllers
{
    
    public class ReservationController : Controller
    {
        
        public async Task<IActionResult> ProcessReservation()
        {
           

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("api/BookingApi");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();
            //ViewBag.Hotels = JObject.Parse(data);
            //return View(data);
            return Content("ok");
        }
    }
}
