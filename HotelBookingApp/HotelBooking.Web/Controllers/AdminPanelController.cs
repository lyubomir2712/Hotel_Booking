using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using HotelBooking.Models.Identity;

namespace HotelBooking.Web.Controllers;

public class AdminPanelController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGetCheckoutedHotelsService _getCheckoutedHotelsService;
    private readonly IAdminPanelDeleteBookingService _adminPanelDeleteBookingService;
    private readonly UserManager<UserModel> _userManager;
    
    public AdminPanelController(IUnitOfWork unitOfWork, IGetCheckoutedHotelsService getCheckoutedHotelsService,
        IAdminPanelDeleteBookingService adminPanelDeleteBookingService, UserManager<UserModel> userManager)
    {
        _getCheckoutedHotelsService = getCheckoutedHotelsService;
        _adminPanelDeleteBookingService = adminPanelDeleteBookingService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCheckoutedHotels()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();
        var checkoutBookings = await _getCheckoutedHotelsService.GetCheckoutedHotels(_unitOfWork, currentUser);
        return View("AdminPanel", checkoutBookings);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminPanelDeleteBooking(int bookingId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null) return Challenge();
        await _adminPanelDeleteBookingService.AdminPanelDeleteBooking(_unitOfWork, bookingId, currentUser);
        return RedirectToAction("GetCheckoutedHotels");
    }
    
    
}