using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class GetRegisteredAccountEmailTemplateHtmlWithParametersService : IGetRegisteredAccountEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template, UserModel receiver, string urlCallback)
    {
        throw new NotImplementedException();
    }
}