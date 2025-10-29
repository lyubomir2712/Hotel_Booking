using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.BookingApiConfigurationContracts;

public interface IGetUnavailableBookingsService
{
    public Task<List<BookingModel>>  VerifyBookingsForAvailability(List<BookingModel> bookings);
}