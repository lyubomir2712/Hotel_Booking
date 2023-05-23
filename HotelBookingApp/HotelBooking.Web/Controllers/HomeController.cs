using HotelBooking.Services.ApiModule;
using HotelBooking.Services.ViewModels;
using HotelBooking.Services.Contracts;
using HotelBooking.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;


namespace HotelBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[HttpPost]
        public IActionResult Hotels(ApiDataViewModel apiDataViewModel)
        {
            //ApiConnectionViewModel apiConnectionDataViewModel = new ApiConnectionViewModel();

            //apiConnectionDataViewModel.DomainName = "https://booking-com.p.rapidapi.com/v1/hotels/locations";
            //apiConnectionDataViewModel.ApiHost = "booking-com.p.rapidapi.com";
            //apiConnectionDataViewModel.ApiKey = "b067df4ec3msh7add19d4e4747fbp12bc39jsna54fc447bbf1";
            //ApiConnectionService connection = new ApiConnectionService(apiConnectionDataViewModel);
            //connection.EstablisConnection();

            //ApiDataViewModel model = new ApiDataViewModel {
            //City = City,
            //CheckinDate = CheckinDate,
            //CheckoutDate = CheckoutDate
            //};


            _apiService.GetHotelsByLocation("https://booking-com.p.rapidapi.com/v1/hotels/locations", apiDataViewModel);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}