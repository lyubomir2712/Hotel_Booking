using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IAddToCartService
{
    public Task AddToCartAsync(IUnitOfWork unitOfWork, AddToCartInput addToCartInput, UserModel currentUser);
}