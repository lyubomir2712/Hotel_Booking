using Microsoft.AspNetCore.Identity.UI.Services;
using LegacyIEmailSender = HotelBooking.Services.Contracts.EmailServicesContracts.IEmailSender;

namespace HotelBooking.Services.EmailServices;

public class IdentityEmailSenderAdapter : IEmailSender
{
    private readonly LegacyIEmailSender _legacy;

    public IdentityEmailSenderAdapter(LegacyIEmailSender legacy)
    {
        _legacy = legacy;
    }
    
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return _legacy.SendAsync(email, subject, htmlMessage);
    }
}