using System.Text.Json.Serialization;

namespace LeadsHub.Core.Payloads.Whatsapp.SendMessage
{
    public sealed class SendMessageContact
    {
        /// <summary>
        /// The whatsapp number to be delivered the Template
        /// </summary>
        public string Input { get; set; } = string.Empty;

        /// <summary>
        /// Id of the whatsapp (whatsapp number)
        /// </summary>
        [JsonPropertyName("wa_id")]
        public string WaId { get; set; } = string.Empty;
    }
}
