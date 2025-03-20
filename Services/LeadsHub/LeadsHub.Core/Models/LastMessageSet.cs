
using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public sealed class LastMessageSet
    {
        public long LeadId { get; set; }

        public long TimelineId { get; set; }

        public string LastMessage { get; set; } = string.Empty;

        public DateTimeOffset LastMessageDate { get; set; }

        public MessageStatus Status { get; set; }

        public MessageSender Sender { get; set; }
    }
}
