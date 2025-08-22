using HotelBooking.Services.Contracts.EmailServicesContracts;
using Microsoft.Extensions.Hosting;

namespace HotelBooking.Services.EmailServices;

public class EmailTemplatePathProviderService(IHostEnvironment env) : IEmailTemplatePathProviderService
{
    private readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "EmailServices", "EmailTemplates");
    public string Checkout => Path.Combine(_basePath, "CheckoutBookingsEmailTemplate.cshtml");
    public string Register  => Path.Combine(_basePath, "RegisterAccountEmailTemplate.cshtml");
}