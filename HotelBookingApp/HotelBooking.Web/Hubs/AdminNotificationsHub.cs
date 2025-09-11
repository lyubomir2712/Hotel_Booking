using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HotelBooking.Web.Hubs;

[Authorize(Roles = "Admin")]
public class AdminNotificationsHub : Hub
{
}