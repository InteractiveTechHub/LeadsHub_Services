using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.Error
{
    public class Error
    {
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Error type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Error code, the whatsapp api cloud
        /// has a complete documentation about error code
        /// </summary>
        public int Code { get; set; }

        [JsonPropertyName("error_data")]
        public ErrorData ErrorData { get; set; } = new();

        /// <summary>
        /// Trace ID, it can be included when contacting support.
        /// </summary>
        [JsonPropertyName("fbtrace_id")]
        public string FbTraceId { get; set; } = string.Empty;
    }
}
