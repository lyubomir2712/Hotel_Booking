using HotelBooking.Data;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class GetHotelsService : IGetHotelsService
{
    public List<HotelModel> GetHotels(BookingDbContext bookingDbContext, List<BookingModel> bookingModel)
    {
        return bookingDbContext.Hotels.Where(b => bookingModel.Select(a => a.Id).Contains(b.Id)).ToList();
    }
}