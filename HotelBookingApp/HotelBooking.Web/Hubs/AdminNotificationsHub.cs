using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HotelBooking.Web.Hubs;

public class AdminNotificationsHub : Hub
{
    private const string AdminGroup = "Admins";

    public override async Task OnConnectedAsync()
    {
        if (Context?.User?.IsInRole("Admin") == true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context?.User?.IsInRole("Admin") == true)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminGroup);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task NotifyAdminForNewBookings(object? payload = null)
    {
        await Clients.Group(AdminGroup).SendAsync("A Customer has made new bookings", payload);
    }
}