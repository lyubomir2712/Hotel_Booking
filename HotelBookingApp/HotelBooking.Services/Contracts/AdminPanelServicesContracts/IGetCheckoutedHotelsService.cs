using HotelBooking.Data.SeedWork;
using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.AdminPanelServicesContracts;

public interface IGetCheckoutedHotelsService
{
    public Task<List<AdminPanelBooking>> GetCheckoutedHotels(UserModel currentUser);
}