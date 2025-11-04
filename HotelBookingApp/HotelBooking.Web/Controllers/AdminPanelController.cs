using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using HotelBooking.Models.Identity;

namespace HotelBooking.Web.Controllers;
[Authorize(Roles = "Admin")]
public class AdminPanelController : Controller
{
    private readonly IGetCheckoutedHotelsService _getCheckoutedHotelsService;
    private readonly IAdminPanelDeleteBookingService _adminPanelDeleteBookingService;
    private readonly UserManager<UserModel> _userManager;
    private readonly IAdminPanelOrderByService _adminPanelOrderByService;
    
    public AdminPanelController(IGetCheckoutedHotelsService getCheckoutedHotelsService,
        IAdminPanelDeleteBookingService adminPanelDeleteBookingService, UserManager<UserModel> userManager,
        IAdminPanelOrderByService adminPanelOrderByService)
    {
        _getCheckoutedHotelsService = getCheckoutedHotelsService;
        _adminPanelDeleteBookingService = adminPanelDeleteBookingService;
        _userManager = userManager;
        _adminPanelOrderByService = adminPanelOrderByService;
    }
    
    public async Task<IActionResult> GetCheckoutedHotels()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();
        var checkoutBookings = await _getCheckoutedHotelsService.GetCheckoutedHotels(currentUser);
        return View("AdminPanel", checkoutBookings);
    }

    
    public async Task<IActionResult> AdminPanelDeleteBooking(int bookingId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();
        await _adminPanelDeleteBookingService.AdminPanelDeleteBooking(bookingId, currentUser);
        return RedirectToAction("GetCheckoutedHotels");
    }


    public async Task<IActionResult> Order(string sortBy, string orderDirection)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();
        
        var orderedBookings = await _adminPanelOrderByService.OrderAdminPanelBookings(sortBy, orderDirection, currentUser);
        return View("AdminPanel", orderedBookings);
    }

}