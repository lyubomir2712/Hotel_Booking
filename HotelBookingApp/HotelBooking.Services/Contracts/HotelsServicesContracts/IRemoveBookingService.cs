using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IRemoveBookingService
{
    public Task RemoveHotelAsync(IUnitOfWork unitOfWork, int hotelId, UserModel currentUser);

}