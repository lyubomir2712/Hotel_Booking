using HotelBooking.Services.EmailServices;

namespace HotelBooking.Services.Contracts.EmailServicesContracts;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, string? textBody = null, IEnumerable<(string fileName, byte[] bytes)>? attachments = null, CancellationToken ct = default);
}