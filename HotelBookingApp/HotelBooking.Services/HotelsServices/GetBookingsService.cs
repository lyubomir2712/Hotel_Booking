using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices;

public class GetBookingsService : IGetBookingsService
{
    public List<BookingModel> GetBookings(BookingDbContext bookingDbContext ,string userId)
    {
        if (bookingDbContext.UserBookings != null)
            return bookingDbContext.UserBookings
                .Where(b => b.UserId == Convert.ToInt32(userId))
                .Include(b => b.BookingModel)
                .ThenInclude(b => b.HotelModel)
                .Select(b => b.BookingModel)
                .ToList();
        return new List<BookingModel>();
    }
}