using HotelBooking.Services.Contracts.AIServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace HotelBooking.Web.Controllers;

public class AiController : Controller
{
    private readonly IAskAppAiService _askAppAiService;
    

    public AiController(IAskAppAiService askAppAiService, IChatClient chatClient)
    {
        _askAppAiService = askAppAiService;
    }
    
    public async Task<IActionResult> AskAi(string question)
    {
        var answer = await _askAppAiService.AskAsync( question);
        return Content(answer);
    }
}