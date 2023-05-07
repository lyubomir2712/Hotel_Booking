using HotelBooking.Models;
using HotelBooking.Services.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace HotelBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            ViewBag.StatusCode = errorCode;
            return this.View();
        }

        public IActionResult SearchedHotels()
        {
            return this.View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminControllPanel()
        {
            return this.View();
        }

        


    }
}