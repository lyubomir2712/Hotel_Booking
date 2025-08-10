using HotelBooking.Data;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers;

public class AdminPanelController : Controller
{
    private BookingDbContext _bookingDbContext;
    private IGetCheckoutedHotelsService _getCheckoutedHotelsService;
    private IAdminPanelDeleteBookingService _adminPanelDeleteBookingService;
    public AdminPanelController(BookingDbContext bookingDbContext, IGetCheckoutedHotelsService getCheckoutedHotelsService,
        IAdminPanelDeleteBookingService adminPanelDeleteBookingService)
    {
        _bookingDbContext = bookingDbContext;
        _getCheckoutedHotelsService = getCheckoutedHotelsService;
        _adminPanelDeleteBookingService = adminPanelDeleteBookingService;
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetCheckoutedHotels()
    {
        var checkoutedBookings = _getCheckoutedHotelsService.GetCheckoutedHotels(_bookingDbContext);
        
        return View("AdminPanel", checkoutedBookings);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminPanelDeleteBooking(int bookingId)
    {
        _adminPanelDeleteBookingService.AdminPanelDeleteBooking(_bookingDbContext, bookingId);
        
        return RedirectToAction("GetCheckoutedHotels");
    }
}