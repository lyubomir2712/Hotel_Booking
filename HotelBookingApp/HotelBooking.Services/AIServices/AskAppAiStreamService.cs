using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.Extensions.AI;

namespace HotelBooking.Services.AI;

public class AskAppAiStreamService : IAskAppAiStreamService
{
    private static readonly IReadOnlyList<ChatMessage> DefaultMessages = new[]
    {
        new ChatMessage(ChatRole.System,
            "Ти си асистент на HotelBooking. Отговаряй кратко и учтиво на български, освен ако потребителят не поиска друго.")
    };
    public IAsyncEnumerable<ChatResponseUpdate> AskStreamAsync(IChatClient chatClient, string prompt, CancellationToken ct = default)
    { 
        // Build history: system + current user message
        var messages = new List<ChatMessage>(DefaultMessages)
        {
            new ChatMessage(ChatRole.User, prompt)
        };
        
        //returns text as a stream, bit by bits, in this case letter by letter
        var partialResponse = chatClient.GetStreamingResponseAsync(prompt, cancellationToken: ct);
        return partialResponse;
    }
}