using HotelBooking.Services.BookingApiConfiguration;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;
using HotelBooking.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

public class HotelsController : Controller
{
    private readonly IApiService _apiService;
    private readonly HttpClient _httpClient;

    public HotelsController(IApiService apiService, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiService = apiService;
    }
    
    public async Task<IActionResult> HotelsSearch(ApiDataViewModel apiDataViewModel)
    {
        // var oldResponse = await _apiService.GetHotelsByLocation(_httpClient, apiDataViewModel);
        var response = new List<BookingViewModel>
        {
            new BookingViewModel
            {
                HotelId = 1,
                Name = "Test Hotel Alpha",
                Country = "USA",
                City = "New York",
                Address = "123 Main Street",
                Latitude = 40.7128,
                Longitude = -74.0060,
                DistanceToCenter = 1.5,
                PhotoMainUrl = "https://picsum.photos/seed/hotel1/800/600",
                Price = 129.99,
                ReviewScore = 8.6,
                ReviewScoreWord = "Excellent",
                StartAt = "2025-08-20",
                EndAt = "2025-08-23",
                ReviewsCount = 312,
                AdultsNumber = 3,
                ChildrenNumber = 1,
                IsFreeCancellable = true,
                IsNoPrepayment = false,
                IncludesBreakfast = true,
                HasFreeParking = true,
                AccommodationType = "Hotel",
                IsBeachFront = false,
                IsPreferred = true,
                IsInBestDistrict = true
            },
            new BookingViewModel
            {
                HotelId = 2,
                Name = "Test Hotel Beta",
                Country = "France",
                City = "Paris",
                Address = "456 Rue de Paris",
                Latitude = 48.8566,
                Longitude = 2.3522,
                DistanceToCenter = 2.3,
                PhotoMainUrl = "https://picsum.photos/seed/hotel2/800/600",
                Price = 89.50,
                ReviewScore = 7.9,
                ReviewScoreWord = "Very good",
                StartAt = "2025-09-01",
                EndAt = "2025-09-04",
                ReviewsCount = 128,
                AdultsNumber = 3,
                ChildrenNumber = 1,
                IsFreeCancellable = false,
                IsNoPrepayment = true,
                IncludesBreakfast = false,
                HasFreeParking = false,
                AccommodationType = "Hostel",
                IsBeachFront = false,

            }
        };
        return View(response);
    }
}