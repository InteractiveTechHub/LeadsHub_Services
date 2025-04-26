
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class SendVideo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;

        [JsonPropertyName("caption")]
        public string Caption { get; set; } = string.Empty;
    }
}
