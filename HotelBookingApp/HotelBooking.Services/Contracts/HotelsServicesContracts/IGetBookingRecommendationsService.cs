using HotelBooking.Models.AppModels;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetBookingRecommendationsService
{
    public Task<List<BookingViewModel>> GetBookingRecomendations(List<BookingModel> unavailableBookings);
}