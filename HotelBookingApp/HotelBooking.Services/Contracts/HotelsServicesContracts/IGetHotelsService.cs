using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.HotelsServicesContracts
{
    public interface IGetHotelsService
    {
        public List<HotelModel> GetHotels(IUnitOfWork unitOfWork, List<BookingModel> bookingModel);
    }
}
