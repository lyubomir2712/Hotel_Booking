namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IGetEmailTemplateFromPathService
{
    public Task<string> GetEmailTemplateFromPath(string emailTemplatePath);
}