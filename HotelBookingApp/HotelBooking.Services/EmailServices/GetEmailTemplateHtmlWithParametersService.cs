using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class GetEmailTemplateHtmlWithParametersService : IGetEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template,UserModel receiver, BookingModel booking)
    {
        return template
            .Replace("{GuestFirstName}", receiver.FirstName)
            .Replace("{BookingId}", booking.Id.ToString())
            .Replace(("{FullName}"), receiver.UserName)
            .Replace("{HotelName}", booking.HotelModel.HotelName);
    }
}