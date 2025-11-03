using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface ICheckoutBookingsService
{
    public Task CheckoutBookingsAsync(UserModel userModel,
        List<BookingModel>? bookingModels);
}