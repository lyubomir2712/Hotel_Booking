using HotelBooking.Data;
using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Services.Contracts.AdminPanelContracts;
using HotelBooking.Services.Contracts.AdminPanelServicesContracts;

namespace HotelBooking.Services.AdminPanelServices;

public class AdminPanelDeleteBookingsService : IAdminPanelDeleteBookingService
{
    public async Task AdminPanelDeleteBooking(IUnitOfWork unitOfWork, int bookingId)
    {
        var adminPanelBooking = await unitOfWork.Repository<AdminPanelBooking>().FindAsync(bookingId);
        if (adminPanelBooking != null)
        {
            await unitOfWork.Repository<AdminPanelBooking>().RemoveAsync(adminPanelBooking);
            await unitOfWork.SaveChangesAsync();
        }
    }
}