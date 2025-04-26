
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class SendAudio
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;
    }
}
