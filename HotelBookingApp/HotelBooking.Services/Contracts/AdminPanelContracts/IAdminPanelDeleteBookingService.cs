using HotelBooking.Data;

namespace HotelBooking.Services.Contracts.AdminPanelContracts;

public interface IAdminPanelDeleteBookingService
{
    public void AdminPanelDeleteBooking(BookingDbContext bookingDbContext, int id);

}