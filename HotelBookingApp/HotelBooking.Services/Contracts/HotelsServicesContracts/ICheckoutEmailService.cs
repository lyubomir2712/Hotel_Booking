using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface ICheckoutEmailService
{
    Task SendCheckoutSummaryAsync(UserModel currentUser, List<BookingModel> bookings);
}