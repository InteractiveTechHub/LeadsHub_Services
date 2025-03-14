using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class TextMessage
    {
        /// <summary>
        /// It's the message to send
        /// </summary>
        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// It allows preview of the url if message has a http link
        /// </summary>
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; } = false;
    }
}