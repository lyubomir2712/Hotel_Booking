using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.HotelsServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.HotelsServices;

public class GetUnavailableBookingHotelNamesFromUserCartService : IGetUnavailableBookingHotelNamesFromUserCartService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetUnavailableBookingHotelNamesFromUserCartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public List<string> GetUnavailableBookingHotelNamesFromUserCart(List<BookingModel> unavailableBookings, UserModel user)
    {
        return _unitOfWork.Repository<UserBookingModel>()
            .Where(ub => ub.UserId == user.Id)
            .Include(ub => ub.BookingModel)
            .ThenInclude(b => b.HotelModel)
            .Select(ub => ub.BookingModel.HotelModel.HotelName)
            .Distinct()
            .AsNoTracking()
            .ToList();
    }
}