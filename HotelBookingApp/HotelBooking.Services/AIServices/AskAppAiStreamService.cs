using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.Extensions.AI;

namespace HotelBooking.Services.AI;

public class AskAppAiStreamService : IAskAppAiStreamService
{
    public IAsyncEnumerable<ChatResponseUpdate> AskStreamAsync(IChatClient chatClient, string prompt, CancellationToken ct = default)
    { 
        //returns text as a stream, bit by bits, in this case letter by letter
        var partialResponse = chatClient.GetStreamingResponseAsync(prompt, cancellationToken: ct);
        return partialResponse;
    }
}