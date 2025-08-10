using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace HotelBooking.Web.Controllers;

public class AiController : Controller
{
    private readonly IAskAppAiService _ai;
    private readonly IChatClient _chatClient;

    public AiController(IAskAppAiService ai, IChatClient chatClient)
    {
        _ai = ai;
        _chatClient = chatClient;
    }
    
    public async Task<IActionResult> Ask(string question)
    {
        var answer = await _ai.AskAsync(_chatClient, question);
        return Content(answer);
    }
}