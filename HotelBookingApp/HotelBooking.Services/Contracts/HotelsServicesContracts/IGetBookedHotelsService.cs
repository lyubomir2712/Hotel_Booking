using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetBookingsService
{
    public Task<List<BookingModel>> GetBookings(IUnitOfWork unitOfWork, string userId);
}