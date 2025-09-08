using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IRegisterAccountEmailService
{
    public Task SendRegisteredAccountService(UserModel newUser, string callbackUrl);
}