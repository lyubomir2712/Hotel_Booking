using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices;

public class GetBookingsService : IGetBookingsService
{
    public List<BookingModel> GetBookings(IUnitOfWork unitOfWork ,string userId)
    {
            return unitOfWork.Repository<UserBookingModel>()
                .Where(b => b.UserId == Convert.ToInt32(userId))
                .Include(b => b.BookingModel)
                .ThenInclude(b => b.HotelModel)
                .Select(b => b.BookingModel)
                .ToList();
    }
}