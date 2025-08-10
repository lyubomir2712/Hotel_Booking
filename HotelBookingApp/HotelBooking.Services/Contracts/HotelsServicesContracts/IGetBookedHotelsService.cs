using HotelBooking.Data;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetBookedHotelsService
{
    public List<BookingModel> GetBookedHotels(BookingDbContext bookingDbContext, string userId);
}