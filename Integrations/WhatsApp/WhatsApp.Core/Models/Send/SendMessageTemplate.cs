using System.Text.Json.Serialization;

namespace WhatsApp.Core.Models.Send
{
    public sealed class SendMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public SendMessageLanguage Language { get; set; } = new();
    }
}