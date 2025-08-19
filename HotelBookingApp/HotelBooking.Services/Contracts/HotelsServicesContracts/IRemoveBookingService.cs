using HotelBooking.Data;
using HotelBooking.Data.SeedWork;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IRemoveBookingService
{
    public Task RemoveHotelAsync(IUnitOfWork unitOfWork, int hotelId);

}