using HotelBooking.Services.ApiModule;
using HotelBooking.Services.StarsService;
using HotelBooking.Services.ViewModels;
using HotelBooking.Services.Contracts;
using HotelBooking.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService _apiService;
        private readonly IStarsService _starsService;
        private readonly BookingDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, IApiService apiService, IStarsService starsService, BookingDbContext dbContext)
        {
            _logger = logger;
            _apiService = apiService;
            _starsService = starsService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        public async Task<IActionResult> Hotels(ApiDataViewModel apiDataViewModel)
        {
            var response =await _apiService.GetHotelsByLocation("https://booking-com.p.rapidapi.com/v1/hotels/locations", "https://booking-com.p.rapidapi.com/v1/hotels/search", apiDataViewModel);
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult ErrorWithStatusCode(int errorCode)
        {
            ViewBag.StatusCode = errorCode;
            return View();
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            var bookings = _dbContext.AdminPanelBookings
                .Include(x => x.HotelModel)
                .ToList();
            return View(bookings);
        }
    }
}