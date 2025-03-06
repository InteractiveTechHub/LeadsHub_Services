
using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Message
{
    public sealed class PayloadReaction
    {
        public string Emoji { get; set; } = string.Empty;

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = string.Empty;
    }
}
