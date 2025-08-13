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
        var response = await _apiService.GetHotelsByLocation(_httpClient, apiDataViewModel);
        return View(response);
    }
}