using HotelBooking.Data;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetBookingsService
{
    public List<BookingModel> GetBookings(BookingDbContext bookingDbContext, string userId);
}