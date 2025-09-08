using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class RegisterAccountEmailService : IRegisterAccountEmailService
{
    
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplatePathProviderService _emailTemplatePathProviderService;
    private readonly IGetEmailTemplateFromPathService _getEmailTemplateFromPathService;
    private readonly IGetCheckoutedBookingsEmailTemplateHtmlWithParametersService _getCheckoutedBookingsEmailTemplateHtmlWithParametersService;

    public RegisterAccountEmailService(
        IEmailSender emailSender,
        IEmailTemplatePathProviderService emailTemplatePathProviderService,
        IGetEmailTemplateFromPathService getEmailTemplateFromPathService,
        IGetCheckoutedBookingsEmailTemplateHtmlWithParametersService getCheckoutedBookingsEmailTemplateHtmlWithParametersService)
    {
        _emailSender = emailSender;
        _emailTemplatePathProviderService = emailTemplatePathProviderService;
        _getEmailTemplateFromPathService = getEmailTemplateFromPathService;
        _getCheckoutedBookingsEmailTemplateHtmlWithParametersService = getCheckoutedBookingsEmailTemplateHtmlWithParametersService;
    }
    
    public async Task SendRegisteredAccountService(UserModel newUser, string callbackUrl)
    {
        if (newUser == null) return;

        var emailReceiver = newUser.Email;
        if (string.IsNullOrWhiteSpace(emailReceiver))
            throw new InvalidOperationException("User does not have a valid email address.");
        
        var subject = "Verify registered account in EasyBook";
        var templatePath = _emailTemplatePathProviderService.RegisterAccountEmailTemplatePath;
        var template = await _getEmailTemplateFromPathService.GetEmailTemplateFromPath(templatePath);
        
        await _emailSender.SendAsync(emailReceiver, subject, template);
    }
}