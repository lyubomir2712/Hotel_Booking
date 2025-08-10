using HotelBooking.Data;
using HotelBooking.Services.Contracts.AdminPanelContracts;

namespace HotelBooking.Services.AdminPanelServices;

public class AdminPanelDeleteBookingsService : IAdminPanelDeleteBookingService
{
    public void AdminPanelDeleteBooking(BookingDbContext bookingDbContext, int bookingId)
    {
        var adminPanelBooking = bookingDbContext.AdminPanelBookings.Find(bookingId);
        if (adminPanelBooking != null)
        {
            bookingDbContext.AdminPanelBookings.Remove(adminPanelBooking);
            bookingDbContext.SaveChanges();
        }
    }
}