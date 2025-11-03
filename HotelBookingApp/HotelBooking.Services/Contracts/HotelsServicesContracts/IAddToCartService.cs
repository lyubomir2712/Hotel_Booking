using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IAddToCartService
{
    public Task AddToCartAsync(AddToCartInput addToCartInput, UserModel currentUser);
}