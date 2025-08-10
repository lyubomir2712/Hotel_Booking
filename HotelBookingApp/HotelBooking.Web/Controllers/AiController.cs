using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace HotelBooking.Web.Controllers;

public class AiController : Controller
{
    private readonly IAskAppAiService _askAppAiService;
    private readonly IChatClient _chatClient;

    public AiController(IAskAppAiService askAppAiService, IChatClient chatClient)
    {
        _askAppAiService = askAppAiService;
        _chatClient = chatClient;
    }
    
    public async Task<IActionResult> Ask(string question)
    {
        var answer = await _askAppAiService.AskAsync(_chatClient, question);
        return Content(answer);
    }
}