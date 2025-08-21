using HotelBooking.Services.Contracts.EmailServicesContracts;

namespace HotelBooking.Services.EmailServices;

public class GetEmailTemplateFromPathService : IGetEmailTemplateFromPathService
{
    public async Task<string> GetEmailTemplateFromPath(string emailTemplatePath)
    {
        return await System.IO.File.ReadAllTextAsync(emailTemplatePath);
    }
}