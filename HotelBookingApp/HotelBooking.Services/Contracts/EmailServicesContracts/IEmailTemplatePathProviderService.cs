namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IEmailTemplatePathProviderService
{ 
    string Checkout { get; } 
    string Register { get; }
}
