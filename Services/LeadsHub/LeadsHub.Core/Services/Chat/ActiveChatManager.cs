
using LeadsHub.Core.Hubs;
using LeadsHub.Core.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace LeadsHub.Core.Services.Chat
{
    /// <summary>
    /// This class is responsible for managing the active chat events.
    /// </summary>
    public class ActiveChatManager : IActiveChatManager
    {
        private readonly ConcurrentDictionary<long, (DateTimeOffset LastMessage, Timer Timer)> _activeChats = new();
        private readonly IHubContext<LeadHub>? _hubContext;

        public ActiveChatManager(IHubContext<LeadHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void AddLead(long leadId, DateTimeOffset lastMessageTime)
        {
            // If timer already exits, cancel before creates a new one
            if (_activeChats.TryRemove(leadId, out var existing))
            {
                existing.Timer.Dispose();
            }

            // The remaining time until reach 24hr
            var timeUntilBlock = TimeSpan.FromHours(24) - (DateTimeOffset.UtcNow - lastMessageTime);

            // It the time already overlaps 24h, block immediatly
            if (timeUntilBlock <= TimeSpan.Zero)
            {
                NotifyBlock(leadId);
                return;
            }

            // Creates a timer to notify when the time expires
            var timer = new Timer(_ => NotifyBlock(leadId), null, timeUntilBlock, Timeout.InfiniteTimeSpan);

            _activeChats[leadId] = (lastMessageTime, timer);
        }

        private void NotifyBlock(long leadId)
        {
            _hubContext?.Clients.Group($"lead-{leadId}").SendAsync("UpdatePermission", leadId, false);
        }

        public bool CanSendFreeMessage(long leadId)
        {
            return _activeChats.TryGetValue(leadId, out var data) &&
                   (DateTime.UtcNow - data.LastMessage).TotalHours < 24;
        }

        public void RemoveLead(long leadId)
        {
            if (_activeChats.TryRemove(leadId, out var existing))
            {
                existing.Timer.Dispose();
            }
        }
    }
}
