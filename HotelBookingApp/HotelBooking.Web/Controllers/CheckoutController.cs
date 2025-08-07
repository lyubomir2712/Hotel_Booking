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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckoutHotels(List<BookingModel> bookings)
    {
        // 1. Вземаме текущия потребител
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null || bookings == null || bookings.Count == 0)
            return RedirectToAction("Index", "Home");

        // 2. Конвертираме всеки BookingModel в AdminPanelBookings
        var adminPanelBookings = bookings.Select(b => new AdminPanelBookings
        {
            ClientId = currentUser.Id,
            ClientFirstName = currentUser.FirstName,   // предполага се, че имаш FirstName в UserModel
            ClientLastName = currentUser.LastName,     // предполага се, че имаш LastName в UserModel
            StartAt = b.StartAt,
            EndAt = b.EndAt,
            Price = b.Price,
            HotelModelId = b.HotelModelId
        }).ToList();

        // 3. Записваме новите AdminPanelBookings
        await _bookingDbContext.AdminPanelBookings.AddRangeAsync(adminPanelBookings);

        // 4. Изтриваме резервациите на текущия потребител
        _bookingDbContext.Bookings.RemoveRange(bookings);

        // 5. Записваме промените
        await _bookingDbContext.SaveChangesAsync();

        // 6. Пренасочваме към „BookedHotels“
        return RedirectToAction("BookedHotels", "BookingsCart");
    }
}