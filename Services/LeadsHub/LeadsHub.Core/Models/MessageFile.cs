
using System.Text.Json.Serialization;

namespace LeadsHub.Core.Models
{
    public class MessageFile
    {
        public long Id { get; set; }

        public string MimeType { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("file_size")]
        public long FileSize { get; set; }

        public bool Animated { get; set; }
    }
}
