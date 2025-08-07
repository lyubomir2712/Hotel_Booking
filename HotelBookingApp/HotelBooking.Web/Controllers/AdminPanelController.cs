using HotelBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers;

public class AdminPanelController : Controller
{
    private BookingDbContext _bookingDbContext;
    public AdminPanelController(BookingDbContext bookingDbContext)
    {
        _bookingDbContext = bookingDbContext;
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetCheckoutedHotels()
    {
        var bookings = _bookingDbContext.AdminPanelBookings
            .Include(x => x.HotelModel)
            .ToList();
        return View("AdminPanel", bookings);
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