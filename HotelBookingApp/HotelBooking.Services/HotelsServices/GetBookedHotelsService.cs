using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices;

public class GetBookedHotelsService : IGetBookedHotelsService
{
    public List<BookingModel> GetBookedHotels(BookingDbContext bookingDbContext ,string userId)
    {
        return bookingDbContext.UserBookings
            .Where(b => b.UserId == Convert.ToInt32(userId))
            .Include(b => b.BookingModel)
            .ThenInclude(b => b.HotelModel)
            .Select(b => b.BookingModel)
            .ToList();
    }
}