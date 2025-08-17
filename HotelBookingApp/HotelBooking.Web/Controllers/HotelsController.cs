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
        // var response = await _apiService.GetHotelsByLocation(_httpClient, apiDataViewModel);
        var response = new List<Hotel>
        {
            new Hotel
            {
                Name = "Test Hotel Alpha",
                PhotoMainUrl = "https://picsum.photos/seed/hotel1/800/600",
                Price = 129.99,
                Currency = "EUR",
                ReviewScore = 8.6,
                ReviewScoreWord = "Excellent",
                Stars = "4",
                StartAt = "2025-08-20",
                EndAt = "2025-08-23",
                ReviewsCount = 312
            },
            new Hotel
            {
                Name = "Test Hotel Beta",
                PhotoMainUrl = "https://picsum.photos/seed/hotel2/800/600",
                Price = 89.50,
                Currency = "EUR",
                ReviewScore = 7.9,
                ReviewScoreWord = "Very good",
                Stars = "3",
                StartAt = "2025-09-01",
                EndAt = "2025-09-04",
                ReviewsCount = 128
            }
        };
        return View(response);
    }
}