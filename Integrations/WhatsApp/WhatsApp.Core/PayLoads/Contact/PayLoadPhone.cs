using System.Text.Json.Serialization;

namespace WhatsApp.Core.PayLoads.Contact
{
    public sealed class PayLoadPhone
    {
        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Whatsapp user Id
        /// </summary>
        [JsonPropertyName("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }
}
