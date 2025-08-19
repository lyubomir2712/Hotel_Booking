using HotelBooking.Data;
using HotelBooking.Data.SeedWork;

namespace HotelBooking.Services.Contracts.AdminPanelContracts;

public interface IAdminPanelDeleteBookingService
{
    public Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int id);

}