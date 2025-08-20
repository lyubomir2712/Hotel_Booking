using HotelBooking.Services.Contracts.EmailServicesContracts;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

public class EmailSenderController : Controller
{
    private readonly IEmailSender _emailSender;

    public EmailSenderController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendEmail(string to, string subject, string body)
    {
        await _emailSender.SendAsync(to, subject, body);
    }
}