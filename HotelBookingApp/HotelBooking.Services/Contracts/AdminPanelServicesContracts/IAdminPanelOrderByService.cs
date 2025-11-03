using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.AdminPanelServicesContracts;

public interface IAdminPanelOrderByService
{
    public Task<List<AdminPanelBooking>> OrderAdminPanelBookings(string orderBy, string orderDirection, UserModel currentUser);
}