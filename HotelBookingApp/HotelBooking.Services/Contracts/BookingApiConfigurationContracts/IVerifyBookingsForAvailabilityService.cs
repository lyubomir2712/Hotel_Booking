using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.BookingApiConfigurationContracts;

public interface IVerifyBookingsForAvailabilityService
{
    public Task<bool> VerifyBookingsForAvailability(HttpClient httpClient, List<BookingModel> bookings);
}