using HotelBooking.Data;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts
{
    public interface IGetHotelsService
    {
        public List<HotelModel> GetHotels(BookingDbContext bookingDbContext, List<BookingModel> bookingModel);
    }
}
