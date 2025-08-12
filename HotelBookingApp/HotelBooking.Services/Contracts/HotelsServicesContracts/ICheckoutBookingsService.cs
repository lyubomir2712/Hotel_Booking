using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface ICheckoutBookingsService
{
    public Task CheckoutBookingsAsync(BookingDbContext bookingDbContext, UserModel userModel,
        List<BookingModel>? bookingModels);
}