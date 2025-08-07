using HotelBooking.Services.Contracts;
using HotelBooking.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

public class HotelsController : Controller
{
    private readonly IApiService _apiService;
    
    public HotelsController(IApiService apiService)
    {
        _apiService = apiService;
    }
    
    public async Task<IActionResult> HotelsSearch(ApiDataViewModel apiDataViewModel)
    {
        var response = await _apiService.GetHotelsByLocation("https://booking-com.p.rapidapi.com/v1/hotels/locations", "https://booking-com.p.rapidapi.com/v1/hotels/search", apiDataViewModel);
        return View(response);
    }
}