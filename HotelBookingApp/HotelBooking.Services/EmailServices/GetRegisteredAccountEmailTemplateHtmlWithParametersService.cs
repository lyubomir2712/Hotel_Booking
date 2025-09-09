using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class GetRegisteredAccountEmailTemplateHtmlWithParametersService : IGetRegisteredAccountEmailTemplateHtmlWithParametersService
{
    public string GetEmailTemplateHtmlWithParameters(string template, UserModel receiver, string callbackUrl)
    {
        var safeUrl = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callbackUrl);
        return 
            template.Replace("{{CallbackUrl}}", safeUrl)
                .Replace("{{UserFirstName}}", receiver.FirstName)
                .Replace("{{VerificationUrl}}", safeUrl)
                .Replace("{{Year}}", DateTime.Now.Year.ToString());
    }
}