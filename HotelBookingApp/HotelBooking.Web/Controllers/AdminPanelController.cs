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
    public AdminPanelController(BookingDbContext bookingDbContext, IGetCheckoutedHotelsService getCheckoutedHotelsService)
    {
        _bookingDbContext = bookingDbContext;
        _getCheckoutedHotelsService = getCheckoutedHotelsService;
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
        var adminPanelBooking = _bookingDbContext.AdminPanelBookings.Find(bookingId);
        if (adminPanelBooking != null)
        {
            _bookingDbContext.AdminPanelBookings.Remove(adminPanelBooking);
            _bookingDbContext.SaveChanges();
        }
        return RedirectToAction("GetCheckoutedHotels");
    }
}