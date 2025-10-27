using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.BookingApiConfigurationContracts;

public interface IGetUnavailableBookingsService
{
    public Task<List<BookingModel>>  VerifyBookingsForAvailability(HttpClient httpClient, List<BookingModel> bookings);
}