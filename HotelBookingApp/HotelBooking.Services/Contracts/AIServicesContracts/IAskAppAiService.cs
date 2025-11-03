using Microsoft.Extensions.AI;

namespace HotelBooking.Services.Contracts.AIServicesContracts;

public interface IAskAppAiService
{
    Task<string> AskAsync(string prompt, CancellationToken ct = default);
}