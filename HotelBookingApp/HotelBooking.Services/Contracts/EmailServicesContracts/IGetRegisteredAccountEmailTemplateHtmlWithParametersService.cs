using HotelBooking.Models.Identity;

namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IGetRegisteredAccountEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template, UserModel receiver,string urlCallback);
}