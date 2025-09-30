using HotelBooking.Data.SeedWork;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.AdminPanelServicesContracts;

public interface IAdminPanelDeleteBookingService
{
    public Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int id, UserModel user);

}