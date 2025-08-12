using HotelBooking.Data;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts;

public interface IRemoveBookingService
{
    public Task RemoveHotelAsync(BookingDbContext bookingDbContext, int hotelId);

}