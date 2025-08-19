using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.HotelsServicesContracts;

namespace HotelBooking.Services.HotelsServices;

public class GetHotelsService : IGetHotelsService
{
    public List<HotelModel> GetHotels(IUnitOfWork unitOfWork, List<BookingModel> bookingModel)
    {
        return unitOfWork.Repository<HotelModel>().Where(b => bookingModel.Select(a => a.Id).Contains(b.Id)).ToList();
    }
}