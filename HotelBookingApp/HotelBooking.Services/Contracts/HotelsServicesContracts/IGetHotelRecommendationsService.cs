using HotelBooking.Models.AppModels;
using HotelBooking.Services.ViewModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IGetHotelRecommendationsService
{
    public Task<List<BookingViewModel>> GetHotelRecomendations(List<BookingModel> unavailableBookings);
}