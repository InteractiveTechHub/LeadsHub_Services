using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class SendMessageTemplate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public SendMessageLanguage Language { get; set; } = new();
    }
}