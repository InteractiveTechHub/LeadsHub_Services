
using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Message
{
    public class PayloadImage
    {
        public string Caption { get; set; } = string.Empty;

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;

        public string Sha256 { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;
    }
}
