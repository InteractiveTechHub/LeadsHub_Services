
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.Response
{
    public sealed class MediaResponse
    {
        [JsonPropertyName("file_size")]
        public string FileSize { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; } = string.Empty;

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;

        public string SHA256 { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

    }
}
