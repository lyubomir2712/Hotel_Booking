using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.Extensions.AI;

namespace HotelBooking.Services.AI
{
    public sealed class AskAppAiService : IAskAppAiService
    {
        public async Task<string> AskAsync(IChatClient chatClient, string prompt, CancellationToken ct = default)
        {
            //return response as one text
            var response = await chatClient.GetResponseAsync(prompt, cancellationToken: ct);
            return response.Text ?? string.Empty;
        }
    }
}