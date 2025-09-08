
namespace LeadsHub.Core.Models
{
    public sealed class MessageReaction : BaseModel
    {
        public string Emoji { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
    }
}
