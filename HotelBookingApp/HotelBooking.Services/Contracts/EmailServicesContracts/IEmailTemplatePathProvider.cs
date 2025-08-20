namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IEmailTemplatePathProvider
{ 
    string Checkout { get; } 
    string Register { get; }
}
