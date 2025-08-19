using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;

namespace HotelBooking.Services.Contracts.AdminPanelContracts;

public interface IGetCheckoutedHotelsService
{
    public List<AdminPanelBooking> GetCheckoutedHotels(IUnitOfWork unitOfWork);
}