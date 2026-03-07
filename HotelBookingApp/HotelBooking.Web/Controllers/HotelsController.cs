using HotelBooking.Services.BookingApiConfiguration;
using HotelBooking.Services.Contracts.BookingApiConfigurationContracts;
using HotelBooking.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

public class HotelsController : Controller
{
    private readonly IApiService _apiService;
    

    public HotelsController(IApiService apiService, HttpClient httpClient)
    {
        _apiService = apiService;
    }
    
    public async Task<IActionResult> HotelsSearch(ApiDataViewModel apiDataViewModel)
    {
        var response = await _apiService.GetHotelsByLocation(apiDataViewModel);
        var testResponse = new List<BookingViewModel>
        {
            new BookingViewModel
            {
                HotelId = 1,
                Name = "Test Hotel",
                Country = "USA",
                City = "New York",
                Address = "123 Main Street",
                Latitude = 40.7128,
                Longitude = -74.0060,
                DistanceToCenter = 1.5,
                PhotoMainUrl = "https://i0.wp.com/ticketseller.gr/wp-content/uploads/2020/10/132354a_hb_a_010.jpg?fit=800%2C600&ssl=1",
                Price = 200.99,
                ReviewScore = 8.6,
                ReviewScoreWord = "Excellent",
                StartAt = "2026-08-15",
                EndAt = "2026-08-16",
                ReviewsCount = 312,
                AdultsNumber = 3,
                ChildrenNumber = 1,
                IsFreeCancellable = true,
                IsNoPrepayment = false,
                IncludesBreakfast = true,
                HasFreeParking = true,
                AccommodationType = "Hotel",
                IsBeachFront = false,
                RoomsNumber = 1,
                DestinationId = "20088325"

            },
            // new BookingViewModel
            // {
            //     HotelId = 2,
            //     Name = "Test Hotel Betaa",
            //     Country = "France",
            //     City = "Paris",
            //     Address = "456 Rue de Paris",
            //     Latitude = 48.8566,
            //     Longitude = 2.3522,
            //     DistanceToCenter = 2.3,
            //     PhotoMainUrl = "https://picsum.photos/seed/hotel2/800/600",
            //     Price = 89.50,
            //     ReviewScore = 7.9,
            //     ReviewScoreWord = "Very good",
            //     StartAt = "2025-12-01",
            //     EndAt = "2025-12-04",
            //     ReviewsCount = 128,
            //     AdultsNumber = 3,
            //     ChildrenNumber = 1,
            //     IsFreeCancellable = false,
            //     IsNoPrepayment = true,
            //     IncludesBreakfast = false,
            //     HasFreeParking = false,
            //     AccommodationType = "Hostel",
            //     IsBeachFront = false,
            //     RoomsNumber = 2,
            //     DestinationId = "20088325"
            // }
        };
        return View(testResponse);
    }
}