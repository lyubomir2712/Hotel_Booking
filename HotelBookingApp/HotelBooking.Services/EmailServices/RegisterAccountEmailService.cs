using HotelBooking.Models.Identity;
using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class RegisterAccountEmailService : IRegisterAccountEmailService
{
    
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplatePathProviderService _emailTemplatePathProviderService;
    private readonly IGetEmailTemplateFromPathService _getEmailTemplateFromPathService;
    private readonly IGetRegisteredAccountEmailTemplateHtmlWithParametersService _getRegisteredAccountEmailTemplateHtmlWithParametersService;

    public RegisterAccountEmailService(
        IEmailSender emailSender,
        IEmailTemplatePathProviderService emailTemplatePathProviderService,
        IGetEmailTemplateFromPathService getEmailTemplateFromPathService,
        IGetRegisteredAccountEmailTemplateHtmlWithParametersService getRegisteredAccountEmailTemplateHtmlWithParametersService)
    {
        _emailSender = emailSender;
        _emailTemplatePathProviderService = emailTemplatePathProviderService;
        _getEmailTemplateFromPathService = getEmailTemplateFromPathService;
        _getRegisteredAccountEmailTemplateHtmlWithParametersService = getRegisteredAccountEmailTemplateHtmlWithParametersService;
    }
    
    public async Task SendRegisteredAccountService(UserModel newUser, string callbackUrl)
    {
        if (newUser == null || callbackUrl == null) return;

        var emailReceiver = newUser.Email;
        if (string.IsNullOrWhiteSpace(emailReceiver))
            throw new InvalidOperationException("User does not have a valid email address.");
        
        var subject = "Verify new account in EasyBook";
        
        var templatePath = _emailTemplatePathProviderService.RegisterAccountEmailTemplatePath;
        
        var template = await _getEmailTemplateFromPathService.GetEmailTemplateFromPath(templatePath);
        
        var templateWithParameters = _getRegisteredAccountEmailTemplateHtmlWithParametersService
            .GetEmailTemplateHtmlWithParameters(template,newUser, callbackUrl);
        
        await _emailSender.SendAsync(emailReceiver, subject, templateWithParameters);
    }
}