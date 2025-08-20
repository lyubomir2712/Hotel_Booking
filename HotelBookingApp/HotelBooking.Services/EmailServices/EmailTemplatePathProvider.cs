using HotelBooking.Services.Contracts.EmailServicesContracts;
using Microsoft.Extensions.Hosting;

namespace HotelBooking.Services.EmailServices;

public class EmailTemplatePathProvider(IHostEnvironment env) : IEmailTemplatePathProvider
{
    private readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "EmailServices", "EmailTemplates");
    public string Checkout => Path.Combine(_basePath, "CheckoutBookingsEmailTemplate.html");
    public string Register  => Path.Combine(_basePath, "RegisterAccountEmailTemplate.html");
}