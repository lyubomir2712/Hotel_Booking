namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IEmailTemplatePathProviderService
{ 
    string CheckoutBookingsEmailTemplatePath { get; } 
    string RegisterAccountEmailTemplatePath { get; }
}
