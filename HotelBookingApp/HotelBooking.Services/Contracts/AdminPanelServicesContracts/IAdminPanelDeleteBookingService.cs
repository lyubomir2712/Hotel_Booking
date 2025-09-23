using HotelBooking.Data.SeedWork;

namespace HotelBooking.Services.Contracts.AdminPanelServicesContracts;

public interface IAdminPanelDeleteBookingService
{
    public Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int id);

}