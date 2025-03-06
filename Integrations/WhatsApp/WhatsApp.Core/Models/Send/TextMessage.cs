using System.Text.Json.Serialization;

namespace WhatsApp.Core.Models.Send
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