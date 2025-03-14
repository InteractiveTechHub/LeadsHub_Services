
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace LeadsHub.Core.Hubs
{
    public class LeadCardHub : Hub
    {
       // private static readonly ConcurrentDictionary<string, string> UserConnections = new();

       // public static IReadOnlyDictionary<string, string> GetConnectedUsers() => UserConnections;

        public override async Task OnConnectedAsync()
        {
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinLeadChatGroup(string leadId)
        {
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(leadId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"lead-{leadId}");
            }
        }

        public async Task LeaveLeadChatGroup(string leadId)
        {
            string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(leadId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"lead-{leadId}");
            }
        }
    }
}
