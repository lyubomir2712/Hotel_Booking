using HotelBooking.Data;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class RemoveBookingService : IRemoveBookingService
{
    public async Task RemoveHotelAsync(BookingDbContext bookingDbContext, int bookingId)
    {
        if (bookingDbContext.Bookings != null)
        {
            var hotel = bookingDbContext.Bookings.First(b => b.Id == bookingId);
        
            bookingDbContext.Bookings.Remove(hotel);
        }

        await bookingDbContext.SaveChangesAsync();
    }
}