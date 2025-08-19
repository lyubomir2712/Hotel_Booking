using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Web.Controllers;

public class AdminPanelController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookingDbContext _bookingDbContext;
    private readonly IGetCheckoutedHotelsService _getCheckoutedHotelsService;
    private readonly IAdminPanelDeleteBookingService _adminPanelDeleteBookingService;
    
    public AdminPanelController(BookingDbContext bookingDbContext,IUnitOfWork unitOfWork, IGetCheckoutedHotelsService getCheckoutedHotelsService,
        IAdminPanelDeleteBookingService adminPanelDeleteBookingService)
    {
        _bookingDbContext = bookingDbContext;
        _getCheckoutedHotelsService = getCheckoutedHotelsService;
        _adminPanelDeleteBookingService = adminPanelDeleteBookingService;
        _unitOfWork = unitOfWork;
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetCheckoutedHotels()
    {
        var checkoutBookings = _getCheckoutedHotelsService.GetCheckoutedHotels(_unitOfWork);
        return View("AdminPanel", checkoutBookings);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminPanelDeleteBooking(int bookingId)
    {
        _adminPanelDeleteBookingService.AdminPanelDeleteBooking(_unitOfWork, bookingId);
        return RedirectToAction("GetCheckoutedHotels");
    }
}