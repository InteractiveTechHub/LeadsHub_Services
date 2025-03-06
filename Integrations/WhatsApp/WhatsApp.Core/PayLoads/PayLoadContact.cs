using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads
{
    public sealed class PayLoadContact
    {
        public PayLoadProfile Profile { get; set; } = new();

        [JsonPropertyName("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }
}