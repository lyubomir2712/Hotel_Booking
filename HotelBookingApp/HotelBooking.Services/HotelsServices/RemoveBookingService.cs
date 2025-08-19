using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class RemoveBookingService : IRemoveBookingService
{
    public async Task RemoveHotelAsync(IUnitOfWork unitOfWork, int bookingId)
    {
        var hotel = await unitOfWork.Repository<BookingModel>().FirstOrDefaultAsync(b => b.Id == bookingId);
        if (hotel != null)
        {
            unitOfWork.Repository<BookingModel>().Remove(hotel);
            await unitOfWork.SaveChangesAsync();
        }
    }
}