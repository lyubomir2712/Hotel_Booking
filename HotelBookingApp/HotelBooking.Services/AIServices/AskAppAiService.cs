using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.Extensions.AI;

namespace HotelBooking.Services.AIServices
{
    public sealed class AskAppAiService : IAskAppAiService
    {
        private readonly IChatClient _chatClient;

        public AskAppAiService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }
        // Predefined chat history (system prompt)
        private static readonly IReadOnlyList<ChatMessage> DefaultMessages = new[]
        {
            new ChatMessage(ChatRole.System,
                "")
        };

        public async Task<string> AskAsync(string prompt, CancellationToken ct = default)
        {
            // Build history: system + current user message
            var messages = new List<ChatMessage>(DefaultMessages)
            {
                new ChatMessage(ChatRole.User, prompt)
            };

            var response = await _chatClient.GetResponseAsync(messages, cancellationToken: ct);
            return response.Text ?? string.Empty;
        }
    }
}