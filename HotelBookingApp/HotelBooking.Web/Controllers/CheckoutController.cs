using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly BookingDbContext _bookingDbContext;
    private readonly UserManager<UserModel> _userManager;

    public CheckoutController(BookingDbContext bookingDbContext, UserManager<UserModel> userManager)
    {
        _bookingDbContext = bookingDbContext;
        _userManager = userManager;
    }
    public async Task<IActionResult> CheckoutHotels(List<BookingModel> bookings)
    {
        List<BookingModel> allBookings = _bookingDbContext.Bookings.ToList();
        var adminRole = "Admin";
        var usersInAdminRole = await _userManager.GetUsersInRoleAsync(adminRole);
        var adminUser = usersInAdminRole.FirstOrDefault();

        // if (adminUser != null)
        // {
        //     foreach (BookingModel booking in allBookings)
        //     {
        //         var userBooking = new UserBookingModel
        //         {
        //             BookingModelId = booking.Id,
        //             UserId = Convert.ToInt32(adminUser.Id)
        //         };
        //     
        //         booking.UserBookingModels.Add(userBooking);
        //     }
        //     
        //     _bookingDbContext.Bookings.AddRange(bookings);
        // }
        
        foreach (BookingModel booking in allBookings)
        {
            _bookingDbContext.Bookings.Remove(booking);
        }
        await _bookingDbContext.SaveChangesAsync();
        return RedirectToAction("BookedHotels", "BookedHotelsByUser");
    }
}