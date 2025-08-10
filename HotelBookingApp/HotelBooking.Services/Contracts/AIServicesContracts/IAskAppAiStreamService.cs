using Microsoft.Extensions.AI;

namespace HotelBooking.Services.Contracts.AIServicesContracts;

public interface IAskAppAiStreamService
{
    IAsyncEnumerable<ChatResponseUpdate> AskStreamAsync(IChatClient chatClient,string prompt, CancellationToken ct = default);
}