using HotelBooking.Data;
using HotelBooking.Models.Identity;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IAddToCartService
{
    public Task AddToCartAsync(BookingDbContext bookingDbContext, AddToCartInput addToCartInput, UserModel currentUser);
}