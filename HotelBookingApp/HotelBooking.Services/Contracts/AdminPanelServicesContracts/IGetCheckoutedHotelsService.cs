using HotelBooking.Data;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.AdminPanelContracts;

public interface IGetCheckoutedHotelsService
{
    public List<AdminPanelBookings> GetCheckoutedHotels(BookingDbContext bookingDbContext);
}