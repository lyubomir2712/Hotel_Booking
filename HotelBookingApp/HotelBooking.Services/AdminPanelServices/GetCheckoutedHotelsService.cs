using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services.AdminPanelServices;

public class GetCheckoutedHotelsService : IGetCheckoutedHotelsService
{
    public List<AdminPanelBooking> GetCheckoutedHotels(IUnitOfWork unitOfWork)
    {
        var bookings = unitOfWork.Repository<AdminPanelBooking>()
            .Include(x => x.HotelModel)
            .ToList();
        
        return bookings;
    }
}