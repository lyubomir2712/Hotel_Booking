using HotelBooking.Models.AppModels;
using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IGetCheckoutedBookingsEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template, UserModel receiver, BookingModel booking, HotelModel hotel);
}