using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IRemoveBookingService
{
    public Task RemoveHotelAsync(int hotelId, UserModel currentUser);

}